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
    private static readonly string[] ColorCodes =
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
        "107",
    ];

    [GeneratedRegex(@"\s")]
    private static partial Regex GetSpaceRegex();

    [GeneratedRegex(@"\[[0-9;]*m")]
    private static partial Regex GetColorEscapePattern();

    private static readonly Regex SpaceRegex = GetSpaceRegex();
    private static readonly Regex ColorEscapePattern = GetColorEscapePattern();

    public static string EscapeLog(string text) =>
        SpaceRegex.Replace(
            WebUtility.HtmlEncode(text).Replace("\r", null).Replace("\n", "<br>"),
            "&nbsp;"
        );

    public static List<LineFragment> ParseAnsiCode(string line)
    {
        if (!line.Contains('\x1b'))
        {
            return [new(line)];
        }

        var list = new List<LineFragment>();

        foreach (var part in line.Split('\x1b', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!ColorEscapePattern.IsMatch(part))
            {
                list.Add(Create(OutputFilter.RemoveANSIEscapeChars("\x1b" + part)));
                continue;
            }

            var fragment = Create(part[(part.IndexOf('m') + 1)..]);
            var args = part[1..part.IndexOf('m')].Split(';');

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "0":
                        fragment.Reset();
                        break;

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
                        {
                            fragment.Styles["color"] =
                                $"rgb({args[i + 2]},{args[i + 3]},{args[i + 4]})";
                        }
                        break;

                    case "48":
                        if (i + 4 <= args.Length && args[i + 1] == "2")
                        {
                            fragment.Styles["background"] =
                                $"rgb({args[i + 2]},{args[i + 3]},{args[i + 4]})";
                        }
                        break;

                    default:
                        if (ColorCodes.Contains(args[i]))
                        {
                            fragment.Classes.Add($"ansi{args[i]}");
                        }
                        break;
                }
            }

            list.Add(fragment);
        }

        return list;

        LineFragment Create(string text)
        {
            return list.Count == 0 ? new LineFragment(text) : new(text, list[^1]);
        }
    }

    public static string ColorLine(string line, OutputStyle outputStyle)
    {
        return outputStyle switch
        {
            OutputStyle.RawText => string.Join(string.Empty, ParseAnsiCode(EscapeLog(line))),
            _ => EscapeLog(OutputFilter.Clean(line)),
        };
    }
}
