using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using EmbedIO;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Web;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Web.Apis;

public static class ApiHelper
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerOptionsFactory.Common)
    {
        Converters =
        {
            new JsonObjectWithIdConverter<Match>(),
            new JsonObjectWithIdConverter<Schedule>(),
        },
    };

    public static async Task<T> ConvertRequestAs<T>(this IHttpContext httpContext)
        where T : notnull
    {
        if (httpContext.Request.HttpVerb is not HttpVerbs.Get or HttpVerbs.Head)
        {
            if (httpContext.Request.ContentType != "application/json")
            {
                throw HttpException.BadRequest("不支持的\"ContentType\"");
            }

            try
            {
                var value = JsonSerializer.Deserialize<T>(
                    await httpContext.GetRequestBodyAsStringAsync(),
                    JsonSerializerOptionsFactory.Common
                );

                return value is null ? throw HttpException.BadRequest("请求体不能为空") : value;
            }
            catch (Exception e)
            {
                throw HttpException.BadRequest(e.Message);
            }
        }

        throw HttpException.MethodNotAllowed();
    }

    public static async Task SendPacketWithEmptyDataAsync(
        this IHttpContext httpContext,
        HttpStatusCode statusCode = HttpStatusCode.OK
    )
    {
        await httpContext.SendPacketAsync<object?>(null, statusCode);
    }

    public static async Task SendPacketAsync<T>(
        this IHttpContext httpContext,
        T? data = default,
        HttpStatusCode statusCode = HttpStatusCode.OK
    )
    {
        await httpContext.SendPacketAsync(data, (int)statusCode);
    }

    public static async Task SendPacketAsync<T>(
        this IHttpContext httpContext,
        T? data,
        int statusCode
    )
    {
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.Headers.Add("X-Serein-Request-Id", httpContext.Id);

        if (statusCode != (int)HttpStatusCode.NoContent && data is not null)
        {
            if (data is ApiPacket packet)
            {
                await httpContext.SendStringAsync(
                    JsonSerializer.Serialize(packet, Options),
                    "text/json",
                    EncodingMap.UTF8
                );
            }
            else
            {
                await httpContext.SendStringAsync(
                    JsonSerializer.Serialize(new ApiPacket { Data = data }, Options),
                    "text/json",
                    EncodingMap.UTF8
                );
            }
        }

        httpContext.SetHandled();
    }

    public static async Task HandleHttpException(IHttpContext context, IHttpException exception)
    {
        await context.SendPacketAsync(
            new ApiPacket
            {
                ErrorMsg =
                    exception.Message
                    ?? HttpStatusDescription.Get(exception.StatusCode)
                    ?? "unknown",
            },
            exception.StatusCode
        );
    }

    public static async Task HandleException(IHttpContext context, Exception e)
    {
        if (e is InvalidOperationException or NotSupportedException or ArgumentException)
        {
            await context.SendPacketAsync(
                new ApiPacket { ErrorMsg = e.Message },
                HttpStatusCode.Forbidden
            );
        }
        else
        {
            var details = new List<string>();

            var internalException = e;

            while (internalException is not null)
            {
                details.Add(
                    internalException.GetType().FullName + ": " + internalException.Message
                );

                if (internalException == internalException.InnerException)
                {
                    break;
                }

                internalException = internalException.InnerException;
            }

            await context.SendPacketAsync(
                new ApiPacket { ErrorMsg = e.Message, Details = [.. details] },
                HttpStatusCode.InternalServerError
            );
        }
    }
}
