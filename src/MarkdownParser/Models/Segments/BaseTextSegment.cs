namespace MarkdownParser.Models.Segments
{
    public abstract class BaseSegment
    {
        public bool HasLiteralContent { get; protected set; } = false;

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
