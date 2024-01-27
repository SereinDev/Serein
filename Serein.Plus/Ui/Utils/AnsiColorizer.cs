using System;
using System.Windows;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace Serein.Plus.Ui.Utils;

public partial class AnsiColorizer : DocumentColorizingTransformer
{
    protected override void ColorizeLine(DocumentLine line)
    {
        var text = CurrentContext.Document.GetText(line);

        if (!text.Contains('\x1b'))
            return;

        int mIndex = -1;

        Color? foreground = null;
        Color? background = null;
        var italic = false;
        var bold = false;
        var underline = false;
        var strikethrough = false;
        var reversed = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '\x1b' || i + 1 >= text.Length || text[i + 1] != '[')
                continue;

            mIndex = text.IndexOf('m', i);

            if (mIndex < 0) // invalid index of 'm'
                continue;

            var args = text.Substring(i + 2, mIndex - i - 2)
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (args.Length == 0)
                continue;

            for (int j = 0; j < args.Length; j++)
            {
                switch (args[j])
                {
                    case "0": // reset
                        foreground = null;
                        background = null;
                        italic = false;
                        bold = false;
                        underline = false;
                        strikethrough = false;
                        reversed = false;
                        break;

                    #region  style

                    case "1":
                        bold = true;
                        break;
                    case "21":
                        bold = false;
                        break;

                    case "3":
                        italic = true;
                        break;
                    case "23":
                        italic = false;
                        break;

                    case "4":
                        underline = true;
                        break;
                    case "24":
                        underline = false;
                        break;

                    case "7":
                        reversed = true;
                        break;
                    case "27":
                        reversed = false;
                        break;

                    case "9":
                        strikethrough = true;
                        break;
                    case "29":
                        strikethrough = false;
                        break;

                    #endregion

                    #region foreground

                    // case "30":
                    //     break;
                    case "31":
                        foreground = ColorMap.Red;
                        break;
                    case "32":
                        foreground = ColorMap.Green;
                        break;
                    case "33":
                        foreground = ColorMap.Yellow;
                        break;
                    case "34":
                        foreground = ColorMap.Blue;
                        break;
                    case "35":
                        foreground = ColorMap.Magenta;
                        break;
                    case "36":
                        foreground = ColorMap.Cyan;
                        break;
                    // case "37":
                    //     break;

                    case "38":
                        var type1 = ColorMap.TryGetColor(args, j + 1, out Color color1);

                        if (type1 == ColorMap.ColorType.EightBit)
                        {
                            foreground = color1;
                            j += 2;
                        }
                        else if (type1 == ColorMap.ColorType.TwentyFourBit)
                        {
                            foreground = color1;
                            j += 3;
                        }
                        break;
                    case "39":
                        foreground = null;
                        break;

                    // case "90":
                    //     break;
                    case "91":
                        foreground = ColorMap.BrightRed;
                        break;
                    case "92":
                        foreground = ColorMap.BrightGreen;
                        break;
                    case "93":
                        foreground = ColorMap.BrightYellow;
                        break;
                    case "94":
                        foreground = ColorMap.BrightBlue;
                        break;
                    case "95":
                        foreground = ColorMap.BrightMagenta;
                        break;
                    case "96":
                        foreground = ColorMap.BrightCyan;
                        break;
                    // case "97":
                    //     break;
                    #endregion

                    #region background

                    // case "40":
                    //     break;
                    case "41":
                        background = ColorMap.Red;
                        break;
                    case "42":
                        background = ColorMap.Green;
                        break;
                    case "43":
                        background = ColorMap.Yellow;
                        break;
                    case "44":
                        background = ColorMap.Blue;
                        break;
                    case "45":
                        background = ColorMap.Magenta;
                        break;
                    case "46":
                        background = ColorMap.Cyan;
                        break;
                    // case "47":
                    //     break;

                    case "48":
                        var type2 = ColorMap.TryGetColor(args, j + 1, out Color color2);

                        if (type2 == ColorMap.ColorType.EightBit)
                        {
                            background = color2;
                            j += 2;
                        }
                        else if (type2 == ColorMap.ColorType.TwentyFourBit)
                        {
                            background = color2;
                            j += 3;
                        }
                        break;
                    case "49":
                        background = null;
                        break;

                    // case "100":
                    //     break;
                    case "101":
                        background = ColorMap.BrightRed;
                        break;
                    case "102":
                        background = ColorMap.BrightGreen;
                        break;
                    case "103":
                        background = ColorMap.BrightYellow;
                        break;
                    case "104":
                        background = ColorMap.BrightBlue;
                        break;
                    case "105":
                        background = ColorMap.BrightMagenta;
                        break;
                    case "106":
                        background = ColorMap.BrightCyan;
                        break;
                        // case "107":
                        //     break;
                        #endregion
                }
            }

            var next = text.IndexOf('\x1b', i + 1);

            ChangeLinePart(
                line.Offset + mIndex + 1,
                line.Offset + (next < 0 ? line.Length : next),
                (element) =>
                    ChangeLine(
                        element,
                        foreground,
                        background,
                        bold,
                        italic,
                        underline,
                        strikethrough,
                        reversed
                    )
            );
        }
    }

    private static void ChangeLine(
        VisualLineElement element,
        Color? foreground,
        Color? background,
        bool bold,
        bool italic,
        bool underline,
        bool strikethrough,
        bool reversed
    )
    {
        if (foreground is not null)
            if (reversed)
                element.TextRunProperties.SetBackgroundBrush(
                    new SolidColorBrush(foreground ?? throw new NullReferenceException())
                );
            else
                element.TextRunProperties.SetForegroundBrush(
                    new SolidColorBrush(foreground ?? throw new NullReferenceException())
                );

        if (background is not null)
            if (reversed)
                element.TextRunProperties.SetForegroundBrush(
                    new SolidColorBrush(background ?? throw new NullReferenceException())
                );
            else
                element.TextRunProperties.SetBackgroundBrush(
                    new SolidColorBrush(background ?? throw new NullReferenceException())
                );

        if (bold || italic)
        {
            var tf = element.TextRunProperties.Typeface;
            element.TextRunProperties.SetTypeface(
                new Typeface(
                    tf.FontFamily,
                    italic ? FontStyles.Italic : FontStyles.Normal,
                    bold ? FontWeights.Bold : FontWeights.Normal,
                    tf.Stretch
                )
            );
        }

        if (strikethrough || underline)
        {
            var decorations = new TextDecorationCollection();

            if (strikethrough)
                decorations.Add(TextDecorations.Strikethrough);

            if (underline)
                decorations.Add(TextDecorations.Underline);

            element.TextRunProperties.SetTextDecorations(decorations);
        }
    }
}
