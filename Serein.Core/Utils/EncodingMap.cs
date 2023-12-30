using System;
using System.Text;

namespace Serein.Core.Utils;

public static class EncodingMap
{
    static EncodingMap()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        UTF8 = new UTF8Encoding(false);
        LittleEndianUnicode = new UnicodeEncoding(false, false);
        BigEndianUnicode = new UnicodeEncoding(true, false);
        GBK = Encoding.GetEncoding("gbk");
    }

    public static readonly Encoding UTF8;
    public static readonly Encoding LittleEndianUnicode;
    public static readonly Encoding BigEndianUnicode;
    public static readonly Encoding GBK;

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
