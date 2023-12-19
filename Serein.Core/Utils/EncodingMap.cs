using System;
using System.Text;

namespace Serein.Core.Utils;

public static class EncodingMap
{
    static EncodingMap()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public static readonly Encoding UTF8 = new UTF8Encoding(false);
    public static readonly Encoding LittleEndianUnicode = new UnicodeEncoding(false, false);
    public static readonly Encoding BigEndianUnicode = new UnicodeEncoding(true, false);
    public static readonly Encoding GBK = Encoding.GetEncoding("gbk");

    public static Encoding GetEncoding(EncodingType encodingType)
    {
        return encodingType switch
        {
            EncodingType.UTF8 => UTF8,
            EncodingType.UTF16BE => BigEndianUnicode,
            EncodingType.UTF16LE => LittleEndianUnicode,
            EncodingType.GBK => GBK,
            _ => throw new ArgumentOutOfRangeException(nameof(encodingType))
        };
    }

    public enum EncodingType
    {
        UTF8,
        UTF16LE,
        UTF16BE,
        GBK
    }
}