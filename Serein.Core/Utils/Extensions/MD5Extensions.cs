using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Serein.Core.Utils.Extensions;

public static class MD5Extensions
{
    public static string CalculateMD5(this string text, Encoding? encoding = null) =>
        GetHexString(MD5.Create().ComputeHash((encoding ?? EncodingMap.UTF8).GetBytes(text)));

    public static string CalculateMD5(this byte[] bytes) =>
        GetHexString(MD5.Create().ComputeHash(bytes));

    public static string CalculateMD5(this Stream stream) =>
        GetHexString(MD5.Create().ComputeHash(stream));

    private static string GetHexString(byte[] targetData)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < targetData.Length; i++)
            stringBuilder.Append(targetData[i].ToString("x2"));

        return stringBuilder.ToString();
    }
}
