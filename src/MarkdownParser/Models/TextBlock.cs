using MarkdownParser.Models.Segments;

namespace MarkdownParser.Models
{
    public class TextBlock
    {
        public BaseSegment[] TextSegments { get; }

        /// <summary>
        /// Ancestors in order of (starting at 0) GrandParent > Parent > Sibling
        /// </summary>
        public BlockType[] AncestorsTree { get; internal set; }
        
        public TextBlock(BaseSegment[] textSegments)
        {
            TextSegments = textSegments;
        }

        /// <summary>
        /// Get all text (LiteralContent) and ignoring all emphasize
        /// </summary>
        /// <param name="usedStringForLineBreaks">text used for line breaks</param>
        /// <returns>clean text</returns>
        public string ExtractLiteralContent(string usedStringForLineBreaks)
        {
            var literalContent = TextSegmentHelper.TextSegmentsToLiteralContent(TextSegments, usedStringForLineBreaks);
            return literalContent;
        }
    }
}