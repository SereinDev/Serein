using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class WebServerHandler(
    ILogger<WebServerHandler> logger,
    WebServer webServer,
    PageExtractor pageExtractor,
    SettingProvider settingProvider
) : CommandHandler
{
    public override string Name { get; } = "网页服务器";

    public override string[] Description { get; } = ["开启或关闭网页服务器", "解压网页文件"];

    public override string RootCommand { get; } = "webserver";

    public override Dictionary<string, string> SubCommands { get; } =
        new()
        {
            ["start"] = "开启网页服务器",
            ["stop"] = "关闭网页服务器",
            ["extract"] = "解压网页文件",
        };

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        switch (subCommand)
        {
            case "start":
                try
                {
                    settingProvider.Value.WebApi.StartWhenSettingUp = true;
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
                    settingProvider.Value.WebApi.StartWhenSettingUp = false;
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
        }
    }
}
