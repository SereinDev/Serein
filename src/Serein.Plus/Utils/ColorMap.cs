using System.Collections.Frozen;
using System.Collections.Generic;
using System.Windows.Media;

namespace Serein.Plus.Utils;

public static class ColorMap
{
    public static (ColorType Type, Color? Color) TryGetColor(string[] args, int offset)
    {
        if (args.Length > offset + 1 && args[offset] == "5")
        {
            if (EightBitColors.TryGetValue(args[offset + 1], out var eightBitColor))
            {
                return (ColorType.EightBit, eightBitColor);
            }
            return (ColorType.Invalid, null);
        }

        if (args.Length > offset + 3 && args[offset] == "2")
        {
            if (
                byte.TryParse(args[offset + 1], out var r)
                && byte.TryParse(args[offset + 2], out var g)
                && byte.TryParse(args[offset + 3], out var b)
            )
            {
                return (ColorType.TwentyFourBit, Color.FromRgb(r, g, b));
            }
            return (ColorType.Invalid, null);
        }

        return (ColorType.Invalid, null);
    }

    public enum ColorType
    {
        Invalid,
        EightBit,
        TwentyFourBit,
    }

    // https://en.wikipedia.org/wiki/ANSI_escape_code#Colors

    public static readonly Color Black = Color.FromRgb(12, 12, 12);
    public static readonly Color Red = Color.FromRgb(197, 15, 31);
    public static readonly Color Green = Color.FromRgb(19, 161, 14);
    public static readonly Color Yellow = Color.FromRgb(193, 156, 0);
    public static readonly Color Blue = Color.FromRgb(0, 55, 218);
    public static readonly Color Magenta = Color.FromRgb(136, 23, 152);
    public static readonly Color Cyan = Color.FromRgb(58, 150, 221);
    public static readonly Color White = Color.FromRgb(204, 204, 204);

    public static readonly Color BrightBlack = Color.FromRgb(118, 118, 118);
    public static readonly Color BrightRed = Color.FromRgb(231, 72, 86);
    public static readonly Color BrightGreen = Color.FromRgb(22, 198, 12);
    public static readonly Color BrightYellow = Color.FromRgb(249, 241, 165);
    public static readonly Color BrightBlue = Color.FromRgb(59, 120, 255);
    public static readonly Color BrightMagenta = Color.FromRgb(180, 0, 158);
    public static readonly Color BrightCyan = Color.FromRgb(97, 214, 214);
    public static readonly Color BrightWhite = Color.FromRgb(242, 242, 242);

    // https://en.wikipedia.org/wiki/ANSI_escape_code#8-bits
    public static readonly IReadOnlyDictionary<string, Color> EightBitColors = new Dictionary<
        string,
        Color
    >
    {
        ["0"] = Color.FromRgb(0x00, 0x00, 0x00),
        ["1"] = Color.FromRgb(0x80, 0x00, 0x00),
        ["2"] = Color.FromRgb(0x00, 0x80, 0x00),
        ["3"] = Color.FromRgb(0x80, 0x80, 0x00),
        ["4"] = Color.FromRgb(0x00, 0x00, 0x80),
        ["5"] = Color.FromRgb(0x80, 0x00, 0x80),
        ["6"] = Color.FromRgb(0x00, 0x80, 0x80),
        ["7"] = Color.FromRgb(0xc0, 0xc0, 0xc0),
        ["8"] = Color.FromRgb(0x80, 0x80, 0x80),
        ["9"] = Color.FromRgb(0xff, 0x00, 0x00),
        ["10"] = Color.FromRgb(0x00, 0xff, 0x00),
        ["11"] = Color.FromRgb(0xff, 0xff, 0x00),
        ["12"] = Color.FromRgb(0x00, 0x00, 0xff),
        ["13"] = Color.FromRgb(0xff, 0x00, 0xff),
        ["14"] = Color.FromRgb(0x00, 0xff, 0xff),
        ["15"] = Color.FromRgb(0xff, 0xff, 0xff),
        ["16"] = Color.FromRgb(0x00, 0x00, 0x00),
        ["17"] = Color.FromRgb(0x00, 0x00, 0x5f),
        ["18"] = Color.FromRgb(0x00, 0x00, 0x87),
        ["19"] = Color.FromRgb(0x00, 0x00, 0xaf),
        ["20"] = Color.FromRgb(0x00, 0x00, 0xd7),
        ["21"] = Color.FromRgb(0x00, 0x00, 0xff),
        ["22"] = Color.FromRgb(0x00, 0x5f, 0x00),
        ["23"] = Color.FromRgb(0x00, 0x5f, 0x5f),
        ["24"] = Color.FromRgb(0x00, 0x5f, 0x87),
        ["25"] = Color.FromRgb(0x00, 0x5f, 0xaf),
        ["26"] = Color.FromRgb(0x00, 0x5f, 0xd7),
        ["27"] = Color.FromRgb(0x00, 0x5f, 0xff),
        ["28"] = Color.FromRgb(0x00, 0x87, 0x00),
        ["29"] = Color.FromRgb(0x00, 0x87, 0x5f),
        ["30"] = Color.FromRgb(0x00, 0x87, 0x87),
        ["31"] = Color.FromRgb(0x00, 0x87, 0xaf),
        ["32"] = Color.FromRgb(0x00, 0x87, 0xd7),
        ["33"] = Color.FromRgb(0x00, 0x87, 0xff),
        ["34"] = Color.FromRgb(0x00, 0xaf, 0x00),
        ["35"] = Color.FromRgb(0x00, 0xaf, 0x5f),
        ["36"] = Color.FromRgb(0x00, 0xaf, 0x87),
        ["37"] = Color.FromRgb(0x00, 0xaf, 0xaf),
        ["38"] = Color.FromRgb(0x00, 0xaf, 0xd7),
        ["39"] = Color.FromRgb(0x00, 0xaf, 0xff),
        ["40"] = Color.FromRgb(0x00, 0xd7, 0x00),
        ["41"] = Color.FromRgb(0x00, 0xd7, 0x5f),
        ["42"] = Color.FromRgb(0x00, 0xd7, 0x87),
        ["43"] = Color.FromRgb(0x00, 0xd7, 0xaf),
        ["44"] = Color.FromRgb(0x00, 0xd7, 0xd7),
        ["45"] = Color.FromRgb(0x00, 0xd7, 0xff),
        ["46"] = Color.FromRgb(0x00, 0xff, 0x00),
        ["47"] = Color.FromRgb(0x00, 0xff, 0x5f),
        ["48"] = Color.FromRgb(0x00, 0xff, 0x87),
        ["49"] = Color.FromRgb(0x00, 0xff, 0xaf),
        ["50"] = Color.FromRgb(0x00, 0xff, 0xd7),
        ["51"] = Color.FromRgb(0x00, 0xff, 0xff),
        ["52"] = Color.FromRgb(0x5f, 0x00, 0x00),
        ["53"] = Color.FromRgb(0x5f, 0x00, 0x5f),
        ["54"] = Color.FromRgb(0x5f, 0x00, 0x87),
        ["55"] = Color.FromRgb(0x5f, 0x00, 0xaf),
        ["56"] = Color.FromRgb(0x5f, 0x00, 0xd7),
        ["57"] = Color.FromRgb(0x5f, 0x00, 0xff),
        ["58"] = Color.FromRgb(0x5f, 0x5f, 0x00),
        ["59"] = Color.FromRgb(0x5f, 0x5f, 0x5f),
        ["60"] = Color.FromRgb(0x5f, 0x5f, 0x87),
        ["61"] = Color.FromRgb(0x5f, 0x5f, 0xaf),
        ["62"] = Color.FromRgb(0x5f, 0x5f, 0xd7),
        ["63"] = Color.FromRgb(0x5f, 0x5f, 0xff),
        ["64"] = Color.FromRgb(0x5f, 0x87, 0x00),
        ["65"] = Color.FromRgb(0x5f, 0x87, 0x5f),
        ["66"] = Color.FromRgb(0x5f, 0x87, 0x87),
        ["67"] = Color.FromRgb(0x5f, 0x87, 0xaf),
        ["68"] = Color.FromRgb(0x5f, 0x87, 0xd7),
        ["69"] = Color.FromRgb(0x5f, 0x87, 0xff),
        ["70"] = Color.FromRgb(0x5f, 0xaf, 0x00),
        ["71"] = Color.FromRgb(0x5f, 0xaf, 0x5f),
        ["72"] = Color.FromRgb(0x5f, 0xaf, 0x87),
        ["73"] = Color.FromRgb(0x5f, 0xaf, 0xaf),
        ["74"] = Color.FromRgb(0x5f, 0xaf, 0xd7),
        ["75"] = Color.FromRgb(0x5f, 0xaf, 0xff),
        ["76"] = Color.FromRgb(0x5f, 0xd7, 0x00),
        ["77"] = Color.FromRgb(0x5f, 0xd7, 0x5f),
        ["78"] = Color.FromRgb(0x5f, 0xd7, 0x87),
        ["79"] = Color.FromRgb(0x5f, 0xd7, 0xaf),
        ["80"] = Color.FromRgb(0x5f, 0xd7, 0xd7),
        ["81"] = Color.FromRgb(0x5f, 0xd7, 0xff),
        ["82"] = Color.FromRgb(0x5f, 0xff, 0x00),
        ["83"] = Color.FromRgb(0x5f, 0xff, 0x5f),
        ["84"] = Color.FromRgb(0x5f, 0xff, 0x87),
        ["85"] = Color.FromRgb(0x5f, 0xff, 0xaf),
        ["86"] = Color.FromRgb(0x5f, 0xff, 0xd7),
        ["87"] = Color.FromRgb(0x5f, 0xff, 0xff),
        ["88"] = Color.FromRgb(0x87, 0x00, 0x00),
        ["89"] = Color.FromRgb(0x87, 0x00, 0x5f),
        ["90"] = Color.FromRgb(0x87, 0x00, 0x87),
        ["91"] = Color.FromRgb(0x87, 0x00, 0xaf),
        ["92"] = Color.FromRgb(0x87, 0x00, 0xd7),
        ["93"] = Color.FromRgb(0x87, 0x00, 0xff),
        ["94"] = Color.FromRgb(0x87, 0x5f, 0x00),
        ["95"] = Color.FromRgb(0x87, 0x5f, 0x5f),
        ["96"] = Color.FromRgb(0x87, 0x5f, 0x87),
        ["97"] = Color.FromRgb(0x87, 0x5f, 0xaf),
        ["98"] = Color.FromRgb(0x87, 0x5f, 0xd7),
        ["99"] = Color.FromRgb(0x87, 0x5f, 0xff),
        ["100"] = Color.FromRgb(0x87, 0x87, 0x00),
        ["101"] = Color.FromRgb(0x87, 0x87, 0x5f),
        ["102"] = Color.FromRgb(0x87, 0x87, 0x87),
        ["103"] = Color.FromRgb(0x87, 0x87, 0xaf),
        ["104"] = Color.FromRgb(0x87, 0x87, 0xd7),
        ["105"] = Color.FromRgb(0x87, 0x87, 0xff),
        ["106"] = Color.FromRgb(0x87, 0xaf, 0x00),
        ["107"] = Color.FromRgb(0x87, 0xaf, 0x5f),
        ["108"] = Color.FromRgb(0x87, 0xaf, 0x87),
        ["109"] = Color.FromRgb(0x87, 0xaf, 0xaf),
        ["110"] = Color.FromRgb(0x87, 0xaf, 0xd7),
        ["111"] = Color.FromRgb(0x87, 0xaf, 0xff),
        ["112"] = Color.FromRgb(0x87, 0xd7, 0x00),
        ["113"] = Color.FromRgb(0x87, 0xd7, 0x5f),
        ["114"] = Color.FromRgb(0x87, 0xd7, 0x87),
        ["115"] = Color.FromRgb(0x87, 0xd7, 0xaf),
        ["116"] = Color.FromRgb(0x87, 0xd7, 0xd7),
        ["117"] = Color.FromRgb(0x87, 0xd7, 0xff),
        ["118"] = Color.FromRgb(0x87, 0xff, 0x00),
        ["119"] = Color.FromRgb(0x87, 0xff, 0x5f),
        ["120"] = Color.FromRgb(0x87, 0xff, 0x87),
        ["121"] = Color.FromRgb(0x87, 0xff, 0xaf),
        ["122"] = Color.FromRgb(0x87, 0xff, 0xd7),
        ["123"] = Color.FromRgb(0x87, 0xff, 0xff),
        ["124"] = Color.FromRgb(0xaf, 0x00, 0x00),
        ["125"] = Color.FromRgb(0xaf, 0x00, 0x5f),
        ["126"] = Color.FromRgb(0xaf, 0x00, 0x87),
        ["127"] = Color.FromRgb(0xaf, 0x00, 0xaf),
        ["128"] = Color.FromRgb(0xaf, 0x00, 0xd7),
        ["129"] = Color.FromRgb(0xaf, 0x00, 0xff),
        ["130"] = Color.FromRgb(0xaf, 0x5f, 0x00),
        ["131"] = Color.FromRgb(0xaf, 0x5f, 0x5f),
        ["132"] = Color.FromRgb(0xaf, 0x5f, 0x87),
        ["133"] = Color.FromRgb(0xaf, 0x5f, 0xaf),
        ["134"] = Color.FromRgb(0xaf, 0x5f, 0xd7),
        ["135"] = Color.FromRgb(0xaf, 0x5f, 0xff),
        ["136"] = Color.FromRgb(0xaf, 0x87, 0x00),
        ["137"] = Color.FromRgb(0xaf, 0x87, 0x5f),
        ["138"] = Color.FromRgb(0xaf, 0x87, 0x87),
        ["139"] = Color.FromRgb(0xaf, 0x87, 0xaf),
        ["140"] = Color.FromRgb(0xaf, 0x87, 0xd7),
        ["141"] = Color.FromRgb(0xaf, 0x87, 0xff),
        ["142"] = Color.FromRgb(0xaf, 0xaf, 0x00),
        ["143"] = Color.FromRgb(0xaf, 0xaf, 0x5f),
        ["144"] = Color.FromRgb(0xaf, 0xaf, 0x87),
        ["145"] = Color.FromRgb(0xaf, 0xaf, 0xaf),
        ["146"] = Color.FromRgb(0xaf, 0xaf, 0xd7),
        ["147"] = Color.FromRgb(0xaf, 0xaf, 0xff),
        ["148"] = Color.FromRgb(0xaf, 0xd7, 0x00),
        ["149"] = Color.FromRgb(0xaf, 0xd7, 0x5f),
        ["150"] = Color.FromRgb(0xaf, 0xd7, 0x87),
        ["151"] = Color.FromRgb(0xaf, 0xd7, 0xaf),
        ["152"] = Color.FromRgb(0xaf, 0xd7, 0xd7),
        ["153"] = Color.FromRgb(0xaf, 0xd7, 0xff),
        ["154"] = Color.FromRgb(0xaf, 0xff, 0x00),
        ["155"] = Color.FromRgb(0xaf, 0xff, 0x5f),
        ["156"] = Color.FromRgb(0xaf, 0xff, 0x87),
        ["157"] = Color.FromRgb(0xaf, 0xff, 0xaf),
        ["158"] = Color.FromRgb(0xaf, 0xff, 0xd7),
        ["159"] = Color.FromRgb(0xaf, 0xff, 0xff),
        ["160"] = Color.FromRgb(0xd7, 0x00, 0x00),
        ["161"] = Color.FromRgb(0xd7, 0x00, 0x5f),
        ["162"] = Color.FromRgb(0xd7, 0x00, 0x87),
        ["163"] = Color.FromRgb(0xd7, 0x00, 0xaf),
        ["164"] = Color.FromRgb(0xd7, 0x00, 0xd7),
        ["165"] = Color.FromRgb(0xd7, 0x00, 0xff),
        ["166"] = Color.FromRgb(0xd7, 0x5f, 0x00),
        ["167"] = Color.FromRgb(0xd7, 0x5f, 0x5f),
        ["168"] = Color.FromRgb(0xd7, 0x5f, 0x87),
        ["169"] = Color.FromRgb(0xd7, 0x5f, 0xaf),
        ["170"] = Color.FromRgb(0xd7, 0x5f, 0xd7),
        ["171"] = Color.FromRgb(0xd7, 0x5f, 0xff),
        ["172"] = Color.FromRgb(0xd7, 0x87, 0x00),
        ["173"] = Color.FromRgb(0xd7, 0x87, 0x5f),
        ["174"] = Color.FromRgb(0xd7, 0x87, 0x87),
        ["175"] = Color.FromRgb(0xd7, 0x87, 0xaf),
        ["176"] = Color.FromRgb(0xd7, 0x87, 0xd7),
        ["177"] = Color.FromRgb(0xd7, 0x87, 0xff),
        ["178"] = Color.FromRgb(0xd7, 0xaf, 0x00),
        ["179"] = Color.FromRgb(0xd7, 0xaf, 0x5f),
        ["180"] = Color.FromRgb(0xd7, 0xaf, 0x87),
        ["181"] = Color.FromRgb(0xd7, 0xaf, 0xaf),
        ["182"] = Color.FromRgb(0xd7, 0xaf, 0xd7),
        ["183"] = Color.FromRgb(0xd7, 0xaf, 0xff),
        ["184"] = Color.FromRgb(0xd7, 0xd7, 0x00),
        ["185"] = Color.FromRgb(0xd7, 0xd7, 0x5f),
        ["186"] = Color.FromRgb(0xd7, 0xd7, 0x87),
        ["187"] = Color.FromRgb(0xd7, 0xd7, 0xaf),
        ["188"] = Color.FromRgb(0xd7, 0xd7, 0xd7),
        ["189"] = Color.FromRgb(0xd7, 0xd7, 0xff),
        ["190"] = Color.FromRgb(0xd7, 0xff, 0x00),
        ["191"] = Color.FromRgb(0xd7, 0xff, 0x5f),
        ["192"] = Color.FromRgb(0xd7, 0xff, 0x87),
        ["193"] = Color.FromRgb(0xd7, 0xff, 0xaf),
        ["194"] = Color.FromRgb(0xd7, 0xff, 0xd7),
        ["195"] = Color.FromRgb(0xd7, 0xff, 0xff),
        ["196"] = Color.FromRgb(0xff, 0x00, 0x00),
        ["197"] = Color.FromRgb(0xff, 0x00, 0x5f),
        ["198"] = Color.FromRgb(0xff, 0x00, 0x87),
        ["199"] = Color.FromRgb(0xff, 0x00, 0xaf),
        ["200"] = Color.FromRgb(0xff, 0x00, 0xd7),
        ["201"] = Color.FromRgb(0xff, 0x00, 0xff),
        ["202"] = Color.FromRgb(0xff, 0x5f, 0x00),
        ["203"] = Color.FromRgb(0xff, 0x5f, 0x5f),
        ["204"] = Color.FromRgb(0xff, 0x5f, 0x87),
        ["205"] = Color.FromRgb(0xff, 0x5f, 0xaf),
        ["206"] = Color.FromRgb(0xff, 0x5f, 0xd7),
        ["207"] = Color.FromRgb(0xff, 0x5f, 0xff),
        ["208"] = Color.FromRgb(0xff, 0x87, 0x00),
        ["209"] = Color.FromRgb(0xff, 0x87, 0x5f),
        ["210"] = Color.FromRgb(0xff, 0x87, 0x87),
        ["211"] = Color.FromRgb(0xff, 0x87, 0xaf),
        ["212"] = Color.FromRgb(0xff, 0x87, 0xd7),
        ["213"] = Color.FromRgb(0xff, 0x87, 0xff),
        ["214"] = Color.FromRgb(0xff, 0xaf, 0x00),
        ["215"] = Color.FromRgb(0xff, 0xaf, 0x5f),
        ["216"] = Color.FromRgb(0xff, 0xaf, 0x87),
        ["217"] = Color.FromRgb(0xff, 0xaf, 0xaf),
        ["218"] = Color.FromRgb(0xff, 0xaf, 0xd7),
        ["219"] = Color.FromRgb(0xff, 0xaf, 0xff),
        ["220"] = Color.FromRgb(0xff, 0xd7, 0x00),
        ["221"] = Color.FromRgb(0xff, 0xd7, 0x5f),
        ["222"] = Color.FromRgb(0xff, 0xd7, 0x87),
        ["223"] = Color.FromRgb(0xff, 0xd7, 0xaf),
        ["224"] = Color.FromRgb(0xff, 0xd7, 0xd7),
        ["225"] = Color.FromRgb(0xff, 0xd7, 0xff),
        ["226"] = Color.FromRgb(0xff, 0xff, 0x00),
        ["227"] = Color.FromRgb(0xff, 0xff, 0x5f),
        ["228"] = Color.FromRgb(0xff, 0xff, 0x87),
        ["229"] = Color.FromRgb(0xff, 0xff, 0xaf),
        ["230"] = Color.FromRgb(0xff, 0xff, 0xd7),
        ["231"] = Color.FromRgb(0xff, 0xff, 0xff),
        ["232"] = Color.FromRgb(0x08, 0x08, 0x08),
        ["233"] = Color.FromRgb(0x12, 0x12, 0x12),
        ["234"] = Color.FromRgb(0x1c, 0x1c, 0x1c),
        ["235"] = Color.FromRgb(0x26, 0x26, 0x26),
        ["236"] = Color.FromRgb(0x30, 0x30, 0x30),
        ["237"] = Color.FromRgb(0x3a, 0x3a, 0x3a),
        ["238"] = Color.FromRgb(0x44, 0x44, 0x44),
        ["239"] = Color.FromRgb(0x4e, 0x4e, 0x4e),
        ["240"] = Color.FromRgb(0x58, 0x58, 0x58),
        ["241"] = Color.FromRgb(0x62, 0x62, 0x62),
        ["242"] = Color.FromRgb(0x6c, 0x6c, 0x6c),
        ["243"] = Color.FromRgb(0x76, 0x76, 0x76),
        ["244"] = Color.FromRgb(0x80, 0x80, 0x80),
        ["245"] = Color.FromRgb(0x8a, 0x8a, 0x8a),
        ["246"] = Color.FromRgb(0x94, 0x94, 0x94),
        ["247"] = Color.FromRgb(0x9e, 0x9e, 0x9e),
        ["248"] = Color.FromRgb(0xa8, 0xa8, 0xa8),
        ["249"] = Color.FromRgb(0xb2, 0xb2, 0xb2),
        ["250"] = Color.FromRgb(0xbc, 0xbc, 0xbc),
        ["251"] = Color.FromRgb(0xc6, 0xc6, 0xc6),
        ["252"] = Color.FromRgb(0xd0, 0xd0, 0xd0),
        ["253"] = Color.FromRgb(0xda, 0xda, 0xda),
        ["254"] = Color.FromRgb(0xe4, 0xe4, 0xe4),
        ["255"] = Color.FromRgb(0xee, 0xee, 0xee),
    }.ToFrozenDictionary();
}
