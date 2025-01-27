using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Serein.Core.Utils;

namespace Serein.Core.Services.Network.Web;

public class PageExtractor(ILogger<PageExtractor> logger)
{
    private const string ResourceName = "Serein.Core.frontend.zip";

    public bool TryExtract()
    {
        Directory.CreateDirectory(PathConstants.WebRoot);

        var assembly = typeof(SereinApp).Assembly;

        using var stream = assembly.GetManifestResourceStream(ResourceName);

        if (stream is null)
        {
            logger.LogWarning("未找到嵌入的页面资源");
            logger.LogWarning(
                "你可以在 https://github.com/SereinDev/Web 仓库下载最新的构建或最新的版本，手动将解压后的文件复制到文件夹“{}”下",
                PathConstants.WebRoot
            );
            return false;
        }

        logger.LogInformation("正在解压页面资源");

        ZipFile.ExtractToDirectory(stream, PathConstants.WebRoot, true);

        logger.LogInformation("解压完毕");
        return true;
    }
}
