using ICSharpCode.AvalonEdit;

using Serein.Plus.Ui.Utils;

namespace Serein.Plus.Utils.Extensions;

public static class TextEditorExtension
{
    public static void EnableAnsiColor(this TextEditor textEditor)
    {
        textEditor.TextArea.TextView.ElementGenerators.Insert(0, new HideAnsiElementGenerator());
        textEditor.TextArea.TextView.LineTransformers.Add(new AnsiColorizer());
    }
}
