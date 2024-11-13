using MarkdownParser.Models.Segments.Indicators;

namespace MarkdownParser.Models.Segments
{
    public class IndicatorSegment : BaseSegment
    {
        public SegmentIndicator Indicator { get; }
        public SegmentIndicatorPosition IndicatorPosition { get; }

        public IndicatorSegment(SegmentIndicator indicator, SegmentIndicatorPosition indicatorPosition)
        {
            Indicator = indicator;
            IndicatorPosition = indicatorPosition;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}