namespace MarkdownParser.Models
{
    public class MarkdownReferenceDefinition
    {
        public string Label { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public bool IsPlaceholder { get; set; }
    }
}