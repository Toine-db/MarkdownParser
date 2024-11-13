using System.Linq;
using System.Text;
using MarkdownParser.Models.Segments;
using MarkdownParser.Models.Segments.Indicators;

namespace MarkdownParser.Models
{
    public static class TextSegmentHelper
    {
        public static string TextSegmentsToLiteralContent(BaseSegment[] segments, string optionalLineBreak)
        {
            var literalContentBuilder = new StringBuilder();

            if (segments == null
                || !segments.Any(x => x.HasLiteralContent))
            {
                return string.Empty;
            }

            for (var i = 0; i < segments.Length; i++)
            {
                var literalContent = segments[i].ToString();
                if (segments[i] is IndicatorSegment indicatorSegment
                    && indicatorSegment.Indicator == SegmentIndicator.LineBreak)
                {
                    literalContent = optionalLineBreak;
                }

                literalContentBuilder.Append(literalContent);
            }

            return literalContentBuilder.ToString();
        }
    }
}