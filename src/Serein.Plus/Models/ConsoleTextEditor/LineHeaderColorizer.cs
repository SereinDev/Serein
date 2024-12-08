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
    private readonly bool _onlySereinHeader = onlySereinHeader;

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
                if (!_onlySereinHeader)
                {
                    action = (element) =>
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0x00, 0xaf, 0xff))
                        );
                }
                break;

            case "Warn":
                if (!_onlySereinHeader)
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
                if (!_onlySereinHeader)
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
                if (!_onlySereinHeader)
                {
                    action = (element) =>
                        element.TextRunProperties.SetForegroundBrush(
                            new SolidColorBrush(Color.FromRgb(0x00, 0x5f, 0xd7))
                        );
                }
                break;

            case "Sent":
            case "↑":
                if (!_onlySereinHeader)
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

        ChangeLinePart(line.Offset, line.Offset + match.Value.Length, action);
    }

    private static readonly Regex HeaderRegex = GetHeaderRegex();

    [GeneratedRegex(@"^\[(?<key>.+?)\]")]
    private static partial Regex GetHeaderRegex();
}
