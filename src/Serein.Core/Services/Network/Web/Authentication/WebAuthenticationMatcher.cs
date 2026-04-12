using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using EmbedIO;
using Serein.Core.Models.Network.Web.WebAuthentication;
using Serein.Core.Models.Settings;

namespace Serein.Core.Services.Network.Web.Authentication;

internal static class WebAuthenticationMatcher
{
    private enum CredentialType
    {
        Unknown,
        Token,
        Digest,
    }

    private const string DefaultDigestRealm = "SereinAuthGate";
    private const int DigestNonceFutureToleranceSeconds = 30;
    private const int DigestNonceMaxAgeSeconds = 300;
    private const int ReplayCleanupInterval = 128;

    private static int s_replayCheckCount;
    private static readonly ConcurrentDictionary<string, DigestReplayState> DigestReplayStates =
        new(StringComparer.Ordinal);

    private static readonly byte[] DigestNonceSecret = RandomNumberGenerator.GetBytes(32);
    private static readonly byte[] DigestOpaqueSecret = RandomNumberGenerator.GetBytes(32);

    private sealed class DigestReplayState
    {
        public object Gate { get; } = new();
        public Dictionary<string, uint> LastNonceCountsByClientNonce { get; } =
            new(StringComparer.OrdinalIgnoreCase);
        public long LastSeenUnixTime { get; set; }
        public HashSet<string> UsedRequestKeys { get; } = new(StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsAuthorized(
        [NotNullWhen(true)] string? authorization,
        HttpVerbs httpVerb,
        string requestUri,
        IReadOnlyCollection<AuthenticationBase> authentications,
        WebReplayProtectionLevel replayProtectionLevel
    )
    {
        if (string.IsNullOrWhiteSpace(authorization))
        {
            return false;
        }

        var credentialType = ResolveCredentialType(authorization);

        foreach (var authentication in authentications)
        {
            try
            {
                switch (credentialType)
                {
                    case CredentialType.Digest
                        when authentication is UserAuthentication userAuthentication
                            && MatchDigest(
                                authorization,
                                httpVerb,
                                requestUri,
                                userAuthentication,
                                replayProtectionLevel
                            ):
                        return true;

                    case CredentialType.Token
                        when authentication is TokenAuthentication tokenAuthentication
                            && MatchToken(authorization, tokenAuthentication):
                        return true;
                }
            }
            catch { }
        }

        return false;
    }

    private static CredentialType ResolveCredentialType(string authorization)
    {
        if (authorization.StartsWith("Digest ", StringComparison.OrdinalIgnoreCase))
        {
            return CredentialType.Digest;
        }

        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return CredentialType.Token;
        }

        return CredentialType.Unknown;
    }

    public static string CreateDigestChallengeHeader()
    {
        var realm = DefaultDigestRealm;
        var nonce = CreateDigestNonce(realm);
        var opaque = CreateDigestOpaque(realm);

        return $"Digest realm=\"{EscapeHeaderValue(realm)}\", qop=\"auth\", nonce=\"{nonce}\", opaque=\"{opaque}\"";
    }

    private static bool MatchToken(string authorization, TokenAuthentication tokenAuthentication)
    {
        if (string.IsNullOrWhiteSpace(tokenAuthentication.Token))
        {
            return false;
        }

        var token = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? authorization[7..]
            : authorization;

        return string.Equals(token, tokenAuthentication.Token, StringComparison.Ordinal);
    }

    private static bool MatchDigest(
        string authorization,
        HttpVerbs httpVerb,
        string requestUri,
        UserAuthentication usernamePassword,
        WebReplayProtectionLevel replayProtectionLevel
    )
    {
        if (!authorization.StartsWith("Digest ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var parameters = ParseDigestParameters(authorization[7..]);

        if (!TryGetRequiredDigestField(parameters, "username", out var username))
        {
            return false;
        }

        if (!string.Equals(username, usernamePassword.Username, StringComparison.Ordinal))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(usernamePassword.Password))
        {
            return false;
        }

        if (!TryGetRequiredDigestField(parameters, "nonce", out var nonce))
        {
            return false;
        }

        if (!TryGetRequiredDigestField(parameters, "uri", out var digestUri))
        {
            return false;
        }

        if (!TryGetRequiredDigestField(parameters, "response", out var response))
        {
            return false;
        }

        if (
            parameters.TryGetValue("realm", out var actualRealm)
            && !string.Equals(actualRealm, DefaultDigestRealm, StringComparison.Ordinal)
        )
        {
            return false;
        }

        if (!ValidateDigestNonce(nonce, DefaultDigestRealm))
        {
            return false;
        }

        if (
            !TryGetRequiredDigestField(parameters, "opaque", out var opaque)
            || !ValidateDigestOpaque(opaque, DefaultDigestRealm)
        )
        {
            return false;
        }

        if (!string.Equals(digestUri, requestUri, StringComparison.Ordinal))
        {
            return false;
        }

        var httpMethod = httpVerb.ToString().ToUpperInvariant();
        var expectedResponse = ComputeDigestResponse(
            username,
            usernamePassword.Password,
            nonce,
            digestUri,
            httpMethod,
            parameters
        );

        if (
            string.IsNullOrEmpty(expectedResponse)
            || !string.Equals(response, expectedResponse, StringComparison.OrdinalIgnoreCase)
        )
        {
            return false;
        }

        return TryRegisterDigestUsage(username, nonce, response, parameters, replayProtectionLevel);
    }

    private static bool TryRegisterDigestUsage(
        string username,
        string nonce,
        string response,
        IReadOnlyDictionary<string, string> parameters,
        WebReplayProtectionLevel replayProtectionLevel
    )
    {
        if (replayProtectionLevel == WebReplayProtectionLevel.Disabled)
        {
            return true;
        }

        CleanupDigestReplayStatesIfNeeded();

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var key = $"{username}:{nonce}";
        var state = DigestReplayStates.GetOrAdd(key, _ => new DigestReplayState());

        lock (state.Gate)
        {
            state.LastSeenUnixTime = now;

            if (!state.UsedRequestKeys.Add($"response:{response}"))
            {
                return false;
            }

            if (replayProtectionLevel == WebReplayProtectionLevel.Normal)
            {
                return true;
            }

            if (
                !parameters.TryGetValue("qop", out var qop)
                || string.IsNullOrWhiteSpace(qop)
                || !string.Equals(qop, "auth", StringComparison.OrdinalIgnoreCase)
            )
            {
                return false;
            }

            if (!parameters.TryGetValue("nc", out var nc) || string.IsNullOrWhiteSpace(nc))
            {
                return false;
            }

            if (
                !parameters.TryGetValue("cnonce", out var cnonce)
                || string.IsNullOrWhiteSpace(cnonce)
            )
            {
                return false;
            }

            if (
                !uint.TryParse(
                    nc,
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture,
                    out var ncValue
                )
            )
            {
                return false;
            }

            if (
                state.LastNonceCountsByClientNonce.TryGetValue(cnonce, out var lastNonceCount)
                && ncValue <= lastNonceCount
            )
            {
                return false;
            }

            state.LastNonceCountsByClientNonce[cnonce] = ncValue;
            return true;
        }
    }

    private static void CleanupDigestReplayStatesIfNeeded()
    {
        var checkCount = System.Threading.Interlocked.Increment(ref s_replayCheckCount);
        if (checkCount % ReplayCleanupInterval != 0)
        {
            return;
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var expirationSeconds = DigestNonceFutureToleranceSeconds + DigestNonceMaxAgeSeconds;

        foreach (var pair in DigestReplayStates)
        {
            if (now - pair.Value.LastSeenUnixTime > expirationSeconds)
            {
                DigestReplayStates.TryRemove(pair.Key, out _);
            }
        }
    }

    private static bool TryGetRequiredDigestField(
        IReadOnlyDictionary<string, string> parameters,
        string key,
        [NotNullWhen(true)] out string? value
    )
    {
        return parameters.TryGetValue(key, out value) && !string.IsNullOrEmpty(value);
    }

    private static string ComputeDigestResponse(
        string username,
        string password,
        string nonce,
        string uri,
        string httpMethod,
        IReadOnlyDictionary<string, string> parameters
    )
    {
        var ha1 = ComputeMd5Hex($"{username}:{DefaultDigestRealm}:{password}");
        var ha2 = ComputeMd5Hex($"{httpMethod}:{uri}");

        if (parameters.TryGetValue("qop", out var qop) && !string.IsNullOrWhiteSpace(qop))
        {
            return
                !parameters.TryGetValue("nc", out var nc)
                || string.IsNullOrWhiteSpace(nc)
                || !parameters.TryGetValue("cnonce", out var cnonce)
                || string.IsNullOrWhiteSpace(cnonce)
                ? string.Empty
                : ComputeMd5Hex($"{ha1}:{nonce}:{nc}:{cnonce}:{qop}:{ha2}");
        }

        return ComputeMd5Hex($"{ha1}:{nonce}:{ha2}");
    }

    private static string ComputeMd5Hex(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = MD5.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string CreateDigestNonce(string realm)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var random = RandomNumberGenerator.GetHexString(12).ToLowerInvariant();
        var signature = ComputeHmacHex(DigestNonceSecret, $"{realm}:{timestamp}:{random}");
        var payload = $"{timestamp}:{random}:{signature}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
    }

    private static bool ValidateDigestNonce(string nonce, string realm)
    {
        string payload;
        try
        {
            payload = Encoding.UTF8.GetString(Convert.FromBase64String(nonce));
        }
        catch (FormatException)
        {
            return false;
        }

        var parts = payload.Split(':');
        if (parts.Length != 3)
        {
            return false;
        }

        if (!long.TryParse(parts[0], out var timestamp))
        {
            return false;
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (
            timestamp > now + DigestNonceFutureToleranceSeconds
            || now - timestamp > DigestNonceMaxAgeSeconds
        )
        {
            return false;
        }

        var expectedSignature = ComputeHmacHex(DigestNonceSecret, $"{realm}:{parts[0]}:{parts[1]}");
        return string.Equals(parts[2], expectedSignature, StringComparison.OrdinalIgnoreCase);
    }

    private static string CreateDigestOpaque(string realm)
    {
        var random = RandomNumberGenerator.GetHexString(12).ToLowerInvariant();
        var signature = ComputeHmacHex(DigestOpaqueSecret, $"{realm}:{random}");
        var payload = $"{random}:{signature}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
    }

    private static bool ValidateDigestOpaque(string opaque, string realm)
    {
        string payload;
        try
        {
            payload = Encoding.UTF8.GetString(Convert.FromBase64String(opaque));
        }
        catch (FormatException)
        {
            return false;
        }

        var parts = payload.Split(':');
        if (parts.Length != 2)
        {
            return false;
        }

        var expectedSignature = ComputeHmacHex(DigestOpaqueSecret, $"{realm}:{parts[0]}");
        return string.Equals(parts[1], expectedSignature, StringComparison.OrdinalIgnoreCase);
    }

    private static string ComputeHmacHex(byte[] key, string value)
    {
        using var hmac = new HMACSHA256(key);
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = hmac.ComputeHash(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string EscapeHeaderValue(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    private static Dictionary<string, string> ParseDigestParameters(string digestPart)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var i = 0;
        while (i < digestPart.Length)
        {
            while (
                i < digestPart.Length && (digestPart[i] == ',' || char.IsWhiteSpace(digestPart[i]))
            )
            {
                i++;
            }

            if (i >= digestPart.Length)
            {
                break;
            }

            var keyStart = i;
            while (i < digestPart.Length && digestPart[i] != '=')
            {
                i++;
            }

            if (i >= digestPart.Length)
            {
                break;
            }

            var key = digestPart[keyStart..i].Trim();
            i++;

            while (i < digestPart.Length && char.IsWhiteSpace(digestPart[i]))
            {
                i++;
            }

            string value;
            if (i < digestPart.Length && digestPart[i] == '"')
            {
                i++;
                var valueBuilder = new StringBuilder();
                while (i < digestPart.Length)
                {
                    if (digestPart[i] == '\\' && i + 1 < digestPart.Length)
                    {
                        i++;
                        valueBuilder.Append(digestPart[i]);
                        i++;
                        continue;
                    }

                    if (digestPart[i] == '"')
                    {
                        i++;
                        break;
                    }

                    valueBuilder.Append(digestPart[i]);
                    i++;
                }

                value = valueBuilder.ToString();
            }
            else
            {
                var valueStart = i;
                while (i < digestPart.Length && digestPart[i] != ',')
                {
                    i++;
                }

                value = digestPart[valueStart..i].Trim();
            }

            if (!string.IsNullOrEmpty(key))
            {
                result[key] = value;
            }

            while (i < digestPart.Length && digestPart[i] != ',')
            {
                i++;
            }
        }

        return result;
    }
}
