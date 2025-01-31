using MarkdownParser.Models.Segments.Indicators;

namespace MarkdownParser.Models.Segments
{
    public class LinkSegment : IndicatorSegment
    {
        public string Url { get; }
        public string Title { get; }

        public LinkSegment(SegmentIndicatorPosition indicatorPosition, string url, string title)
            : base(SegmentIndicator.Link, indicatorPosition)
        {
            Url = url;
            Title = title;
        }
    }
}