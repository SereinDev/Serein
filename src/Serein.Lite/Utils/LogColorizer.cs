using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

using Serein.Core.Models.Server;
using Serein.Core.Utils;
using Serein.Lite.Models;

namespace Serein.Lite.Utils;

public static partial class LogColorizer
{
    private static readonly string[] ColorList =
    [
        "30",
        "31",
        "32",
        "33",
        "34",
        "35",
        "36",
        "37",
        "40",
        "41",
        "42",
        "43",
        "44",
        "45",
        "46",
        "47",
        "90",
        "91",
        "92",
        "93",
        "94",
        "95",
        "96",
        "97",
        "100",
        "101",
        "102",
        "103",
        "104",
        "105",
        "106",
        "107"
    ];

    [GeneratedRegex(@"\s")]
    private static partial Regex SpaceRegex();

    [GeneratedRegex(@"\x1b\[([^\x1b]+?)m([^\x1b]*)")]
    private static partial Regex AnsiRegex();

    public static string EscapeLog(string text) =>
        SpaceRegex().Replace(
            WebUtility.HtmlEncode(text).Replace("\r", null).Replace("\n", "<br>"),
            "&nbsp;"
            );

    public static List<LineFragment> ParseAnsiCode(string line)
    {
        if (!line.Contains('\x1b'))
            return new() { new(line) };

        var list = new List<LineFragment>();
        foreach (Match match in AnsiRegex().Matches(line.Replace("\x1b[m", "\x1b[0m")))
        {
            if (string.IsNullOrEmpty(match.Groups[2].Value))
                continue;

            var fragment = new LineFragment(match.Groups[2].Value);
            var args = match.Groups[1].Value.Split(';');

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "1":
                        fragment.SetBold();
                        break;

                    case "3":
                        fragment.SetItalic();
                        break;

                    case "4":
                        fragment.SetUnderline();
                        break;

                    case "38":
                        if (i + 4 <= args.Length && args[i + 1] == "2")
                            fragment.Styles["color"] =
                                $"rgb({args[i + 2]},{args[i + 3]},{args[i + 4]})";
                        break;

                    case "48":
                        if (i + 4 <= args.Length && args[i + 1] == "2")
                            fragment.Styles["background"] =
                                $"rgb({args[i + 2]},{args[i + 3]},{args[i + 4]})";
                        break;

                    default:
                        if (ColorList.Contains(args[i]))
                            fragment.Classes.Add($"ansi{args[i]}");
                        break;
                }
            }

            list.Add(fragment);
        }

        return list;
    }

    public static string ColorLine(string line, OutputStyle outputStyle)
    {
        return outputStyle switch
        {
            OutputStyle.RawText => string.Join(string.Empty, ParseAnsiCode(EscapeLog(line))),
            _ => EscapeLog(OutputFilter.Clear(line)),
        };
    }
}
