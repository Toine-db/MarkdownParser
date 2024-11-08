namespace MarkdownParser.Models.Segments
{
    public class LinkSegment : IndicatorSegment
    {
        public string Url { get; }
        public string Title { get; }

        public LinkSegment(SegmentIndicator indicator, SegmentIndicatorPosition indicatorPosition, string url, string title)
            : base(indicator, indicatorPosition)
        {
            Url = url;
            Title = title;
        }
    }
}