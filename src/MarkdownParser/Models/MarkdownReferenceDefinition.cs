namespace MarkdownParser.Models
{
    public class MarkdownReferenceDefinition
    {
        public string PlaceholderId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public bool IsPlaceholder { get; set; }
    }
}