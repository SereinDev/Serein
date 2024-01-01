using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Serein.Core.Utils;

public static class Encryption
{
    public static string GetMD5(string text) =>
        GetHexString(MD5.Create().ComputeHash(EncodingMap.UTF8.GetBytes(text)));

    public static string GetMD5(byte[] bytes) => GetHexString(MD5.Create().ComputeHash(bytes));

    public static string GetMD5(Stream stream) => GetHexString(MD5.Create().ComputeHash(stream));

    public static string GetHexString(byte[] targetData)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < targetData.Length; i++)
        {
            stringBuilder.Append(targetData[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }
}
