using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serein.Cli.Models;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("webserver", "网页服务器")]
[CommandDescription(["开启或关闭网页服务器", "解压网页文件"])]
[SubCommand("start", "开启网页服务器")]
[SubCommand("stop", "关闭网页服务器")]
[SubCommand("extract", "解压网页文件")]
public sealed class WebServerHandler(
    ILogger<WebServerHandler> logger,
    WebServer webServer,
    PageExtractor pageExtractor,
    SettingProvider settingProvider
) : CommandHandler
{
    public override void Invoke(IReadOnlyList<string> args)
    {
        if (args.Count == 1)
        {
            throw new InvalidArgumentException(
                "缺少参数。可用值：\"start\"、\"stop\"和\"extract\""
            );
        }

        switch (args[1].ToLowerInvariant())
        {
            case "start":
                try
                {
                    settingProvider.Value.WebApi.IsEnabled = true;
                    webServer.Start();
                }
                catch (Exception e)
                {
                    logger.LogError("网页服务器启动失败：{}", e.Message);
                }
                break;

            case "stop":
                try
                {
                    settingProvider.Value.WebApi.IsEnabled = false;
                    webServer.Stop();
                }
                catch (Exception e)
                {
                    logger.LogError("网页服务器关闭失败：{}", e.Message);
                }
                break;

            case "extract":
                try
                {
                    pageExtractor.Extract();
                    logger.LogInformation("网页文件解压成功，需要重启网页服务器以应用更改");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "网页文件解压失败");
                }
                break;

            default:
                throw new InvalidArgumentException(
                    "未知的参数。可用值：\"start\"、\"stop\"和\"extract\""
                );
        }
    }
}
