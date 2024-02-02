using System.Diagnostics;

using ICSharpCode.AvalonEdit.Rendering;

namespace Serein.Plus.Ui.Utils;

public class HideLogLevelPrefixElementGenerator : VisualLineElementGenerator
{
    public override VisualLineElement ConstructElement(int offset)
    {
        return new HiddenTextElement(1);
    }

    public override int GetFirstInterestedOffset(int startOffset)
    {
        int endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
        var relevantText = CurrentContext.GetText(startOffset, endOffset - startOffset);
        var text = relevantText.Text;

        int i = text.IndexOf(LogLevelColorizer.Prefix, relevantText.Offset, relevantText.Count);

        if (i == 0)
            return startOffset;
        else if (i > 0)
            if (text[i - 1] == '\r' || text[i - 1] == '\n')
                return i - relevantText.Offset + startOffset;

        return -1;
    }
}
