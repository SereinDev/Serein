using System.Windows.Media.TextFormatting;
using ICSharpCode.AvalonEdit.Rendering;

namespace Serein.Plus.Models.ConsoleTextEditor;

public class HiddenTextElement(int documentLength) : VisualLineElement(1, documentLength)
{
    public override TextRun CreateTextRun(
        int startVisualColumn,
        ITextRunConstructionContext context
    ) => TextHiddenElement;

    private static readonly TextHidden TextHiddenElement = new(1);
}
