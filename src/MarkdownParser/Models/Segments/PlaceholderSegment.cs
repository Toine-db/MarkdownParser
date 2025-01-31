using MarkdownParser.Models.Segments.Indicators;

namespace MarkdownParser.Models.Segments
{
    public class PlaceholderSegment : IndicatorSegment
    {
        public string Url { get; }
        public string Title { get; }

        public PlaceholderSegment(string url, string title)
            : base(SegmentIndicator.Placeholder, SegmentIndicatorPosition.Start)
        {
            Url = url;
            Title = title;
            HasLiteralContent = !string.IsNullOrWhiteSpace(Title) 
                                && !string.IsNullOrWhiteSpace(Url);
        }

        public override string ToString()
        {
            return Title ?? Url ?? string.Empty;
        }
    }
}