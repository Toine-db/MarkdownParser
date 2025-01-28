namespace MarkdownParser.Models
{
    public enum BlockType
    {
        RootOrUnknown,
        List, 
        FencedCode, 
        IndentedCode, 
        Html, 
        Quote, 
        Heading, 
        Paragraph
    }
}