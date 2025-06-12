using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace Serein.Plus.Models.ConsoleTextEditor;

public partial class LineHeaderColorizer(bool onlySereinHeader = false)
    : DocumentColorizingTransformer
{
    protected override void ColorizeLine(DocumentLine line)
    {
        var text = CurrentContext.Document.GetText(line);

        if (!text.Contains('[') || !text.Contains(']'))
        {
            return;
        }

        var match = HeaderRegex.Match(text);
        if (!match.Success)
        {
            return;
        }

        var key = match.Groups["key"].Value;

        Action<VisualLineElement>? action = null;

        switch (key)
        {
            case nameof(Serein):
                action = (element) =>
                    element.TextRunProperties.SetForegroundBrush(
                        new SolidColorBrush(Color.FromRgb(0x4B, 0x73, 0x8D))
                    );
                break;

            case "Info":
                if (!onlySereinHeader)
                {
                    action = (element) =>
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0x00, 0xaf, 0xff))
                        );
                }
                break;

            case "Debug":
                if (!onlySereinHeader)
                {
                    action = (element) =>
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0x8d, 0x72, 0xb5))
                        );
                }
                break;

            case "Warn":
                if (!onlySereinHeader)
                {
                    action = (element) =>
                    {
                        var tf = element.TextRunProperties.Typeface;
                        element.TextRunProperties.SetTypeface(
                            new Typeface(
                                tf.FontFamily,
                                FontStyles.Normal,
                                FontWeights.Bold,
                                tf.Stretch
                            )
                        );
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0xaf, 0x5f, 0x00))
                        );
                    };
                }
                break;

            case "Error":
                if (!onlySereinHeader)
                {
                    action = (element) =>
                    {
                        var tf = element.TextRunProperties.Typeface;
                        element.TextRunProperties.SetTypeface(
                            new Typeface(
                                tf.FontFamily,
                                FontStyles.Normal,
                                FontWeights.Bold,
                                tf.Stretch
                            )
                        );
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0xd7, 0x00, 0x00))
                        );
                    };
                }
                break;

            case "Recv":
            case "↓":
                if (!onlySereinHeader)
                {
                    action = (element) =>
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0x00, 0x5f, 0xd7))
                        );
                }
                break;

            case "Sent":
            case "↑":
                if (!onlySereinHeader)
                {
                    action = (element) =>
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0x5f, 0xd7, 0x00))
                        );
                }
                break;

            default:
                return;
        }

        if (action is not null)
        {
            ChangeLinePart(line.Offset, line.Offset + match.Value.Length, action);
        }
    }

    private static readonly Regex HeaderRegex = GetHeaderRegex();

    [GeneratedRegex(@"^\[(?<key>.+?)\]")]
    private static partial Regex GetHeaderRegex();
}
