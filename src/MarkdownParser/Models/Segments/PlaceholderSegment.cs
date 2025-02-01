using MarkdownParser.Models.Segments.Indicators;

namespace MarkdownParser.Models.Segments
{
    public class PlaceholderSegment : IndicatorSegment
    {
        public string PlaceholderId { get; }
        public string Title { get; }

        public PlaceholderSegment(string placeholderId, string title)
            : base(SegmentIndicator.Placeholder, SegmentIndicatorPosition.Start)
        {
            PlaceholderId = placeholderId?.ToUpper();
            Title = title;
            HasLiteralContent = !string.IsNullOrWhiteSpace(Title) 
                                && !string.IsNullOrWhiteSpace(placeholderId);
        }

        public override string ToString()
        {
            return Title ?? PlaceholderId ?? string.Empty;
        }
    }
}