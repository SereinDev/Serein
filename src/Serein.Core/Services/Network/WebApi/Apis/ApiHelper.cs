using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using EmbedIO;

using Serein.Core.Models.Network.WebApi;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.WebApi.Apis;

public static class ApiHelper
{
    public static async Task<T?> ConvertRequestAs<T>(this IHttpContext httpContext)
        where T : notnull
    {
        if (httpContext.Request.HttpVerb is not HttpVerbs.Get or HttpVerbs.Head)
        {
            if (httpContext.Request.ContentType != "application/json")
                throw HttpException.BadRequest("不支持的\"ContentType\"");

            try
            {
                return JsonSerializer.Deserialize<T>(
                    await httpContext.GetRequestBodyAsStringAsync(),
                    JsonSerializerOptionsFactory.CamelCase
                );
            }
            catch (Exception e)
            {
                throw HttpException.BadRequest(e.Message);
            }
        }

        throw HttpException.MethodNotAllowed();
    }

    private static async Task SendApiPacketAsync<T>(
        this IHttpContext httpContext,
        ApiPacket<T> packet
    )
    {
        httpContext.Response.StatusCode = packet.Code;
        await httpContext.SendStringAsync(
            JsonSerializer.Serialize(packet, JsonSerializerOptionsFactory.CamelCase),
            "text/json",
            EncodingMap.UTF8
        );

        httpContext.SetHandled();
    }

    private static async Task SendPacketAsync(this IHttpContext httpContext, ApiPacket packet) =>
        await SendApiPacketAsync(httpContext, packet);

    public static async Task SendPacketAsync<T>(
        this IHttpContext httpContext,
        T? data = default,
        HttpStatusCode statusCode = HttpStatusCode.OK
    )
        where T : notnull
    {
        await SendApiPacketAsync(
            httpContext,
            new ApiPacket<T>() { Data = data, Code = (int)statusCode }
        );
    }

    public static async Task HandleHttpException(IHttpContext context, IHttpException exception)
    {
        await context.SendPacketAsync(
            new ApiPacket
            {
                ErrorMsg = exception.Message
                        ?? HttpStatusDescription.Get(exception.StatusCode)
                        ?? "Unknown",
                Code = exception.StatusCode
            }
        );
    }

    public static async Task HandleException(IHttpContext context, Exception e)
    {
        await context.SendPacketAsync(
            new ApiPacket { ErrorMsg = e.GetDetailString(), Code = 500 }
        );
    }
}
