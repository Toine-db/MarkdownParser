namespace MarkdownParser.Models.Segments
{
    public class TextSegment : BaseSegment
    {
        public string Text { get; set; }

        public TextSegment(string text)
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