namespace MarkdownParser.Models.Segments
{
    public sealed class Segment : BaseSegment
    {
        public string Text { get; set; }

        public Segment(string text)
        {
            Text = text;
            HasLiteralContent = !string.IsNullOrWhiteSpace(Text);
        }

        public override string ToString()
        {
            return Text ?? string.Empty;
        }
    }
}