using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Serein.Core.Utils.Extensions;

public static class MD5Extensions
{
    public static string CalculateMD5(this string text, Encoding? encoding = null) =>
        GetHexString(MD5.HashData((encoding ?? EncodingMap.UTF8).GetBytes(text)));

    public static string CalculateMD5(this byte[] bytes) =>
        GetHexString(MD5.HashData(bytes));

    public static string CalculateMD5(this Stream stream) =>
        GetHexString(MD5.Create().ComputeHash(stream));

    public static string GetHexString(this byte[] targetData)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < targetData.Length; i++)
            stringBuilder.Append(targetData[i].ToString("x2"));

        return stringBuilder.ToString();
    }
}
