using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Serein.Core.Services.Network.Web;

internal sealed class WebSocketTicketService
{
    public const int DefaultTtlSeconds = 60;

    private static readonly byte[] Secret = RandomNumberGenerator.GetBytes(32);
    private readonly ConcurrentDictionary<string, byte> _usedTickets = new(StringComparer.Ordinal);

    public string IssueTicket(string path)
    {
        var normalizedPath = NormalizePath(path);
        var expiresAt = DateTimeOffset.UtcNow.AddSeconds(DefaultTtlSeconds).ToUnixTimeSeconds();
        var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var payload = $"{normalizedPath}\n{expiresAt}\n{nonce}";

        var payloadBytes = Encoding.UTF8.GetBytes(payload);
        var signature = Hmac(payloadBytes);

        return Base64UrlEncode(payloadBytes) + "." + Base64UrlEncode(signature);
    }

    public bool ValidateTicket(string ticket, string path)
    {
        if (string.IsNullOrWhiteSpace(ticket))
        {
            return false;
        }

        var separatorIndex = ticket.IndexOf('.');
        if (separatorIndex <= 0 || separatorIndex == ticket.Length - 1)
        {
            return false;
        }

        var payloadPart = ticket[..separatorIndex];
        var signaturePart = ticket[(separatorIndex + 1)..];

        byte[] payloadBytes;
        byte[] signatureBytes;

        try
        {
            payloadBytes = Base64UrlDecode(payloadPart);
            signatureBytes = Base64UrlDecode(signaturePart);
        }
        catch (FormatException)
        {
            return false;
        }

        var expectedSignature = Hmac(payloadBytes);
        if (!CryptographicOperations.FixedTimeEquals(signatureBytes, expectedSignature))
        {
            return false;
        }

        var payload = Encoding.UTF8.GetString(payloadBytes);
        var lines = payload.Split('\n');
        if (lines.Length != 3)
        {
            return false;
        }

        if (!long.TryParse(lines[1], out var expiresAt))
        {
            return false;
        }

        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expiresAt)
        {
            return false;
        }

        var normalizedPath = NormalizePath(path);
        if (!string.Equals(lines[0], normalizedPath, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // One-time consumption avoids replay when ticket leaks via URL logs/history.
        return _usedTickets.TryAdd(ticket, 0);
    }

    private static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("路径不能为空", nameof(path));
        }

        var trimmed = path.Trim();

        if (!trimmed.StartsWith('/'))
        {
            trimmed = '/' + trimmed;
        }

        return trimmed;
    }

    private static byte[] Hmac(byte[] payload)
    {
        using var hmac = new HMACSHA256(Secret);
        return hmac.ComputeHash(payload);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    private static byte[] Base64UrlDecode(string value)
    {
        var normalized = value.Replace('-', '+').Replace('_', '/');

        switch (normalized.Length % 4)
        {
            case 2:
                normalized += "==";
                break;
            case 3:
                normalized += "=";
                break;
            case 1:
                throw new FormatException("Invalid base64url length.");
        }

        return Convert.FromBase64String(normalized);
    }
}
