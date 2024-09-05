using ICSharpCode.AvalonEdit.Rendering;

namespace Serein.Plus.Utils;

/// <summary>
/// Gags ANSI color codes and control sequences.
/// </summary>
/// <remarks>
/// Written by: Blake Pell, blakepell@hotmail.com
/// </remarks>
public class HideAnsiElementGenerator : VisualLineElementGenerator
{
    public bool ParseExtended { get; set; } = true;

    /// <summary>
    /// A basic list of ANSI control sequences.
    /// </summary>
    private static readonly char[] EndMarkers = ['m', 'A', 'B', 'C', 'D', 'J'];

    /// <summary>
    /// A more extensive list of the ANSI control sequences.
    /// </summary>
    private static readonly char[] EndMarkersExtended =
    [
        'm',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'J',
        'K',
        'S',
        'T'
    ];

    /// <summary>
    /// A match of a control sequence containing only the match's offset and the length
    /// to the end of the sequence.
    /// </summary>
    private struct Match
    {
        public int MatchOffset;
        public int Length;
    }

    /// <summary>
    /// Finds the position and length of the next ANSI sequence starting at the specified offset.
    /// </summary>
    /// <param name="startOffset"></param>
    private Match FindPosition(int startOffset)
    {
        int endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
        var relevantText = CurrentContext.GetText(startOffset, endOffset - startOffset);
        int index = relevantText.Text.IndexOf('\x1B', relevantText.Offset, relevantText.Count);

        var m = new Match
        {
            MatchOffset = index > -1 ? index - relevantText.Offset + startOffset : -1
        };

        if (index == -1)
        {
            return m;
        }

        int endIndex = relevantText.Text.IndexOfAny(
            ParseExtended ? EndMarkersExtended : EndMarkers,
            index
        );

        if (endIndex > -1)
        {
            endIndex++;
            m.Length = endIndex - index;
        }
        else
        {
            // There was no end to this sequence which might be a partial sequence.
            m.MatchOffset = -1;
        }

        return m;
    }

    /// <inheritdoc />
    public override int GetFirstInterestedOffset(int startOffset)
    {
        // This will return either the offset or a -1 if it's not found as per the AvalonEdit docs.
        return FindPosition(startOffset).MatchOffset;
    }

    /// <inheritdoc/>
    public override VisualLineElement ConstructElement(int offset)
    {
        var m = FindPosition(offset);

        // The offset must have found a match and be equal to the offset specified
        // in order to have the HiddenTextElement created, otherwise we ignore it
        // and return a null from here.
        return m.MatchOffset > -1 && m.MatchOffset == offset
            ? new HiddenTextElement(m.Length)
            : null!;
    }
}
