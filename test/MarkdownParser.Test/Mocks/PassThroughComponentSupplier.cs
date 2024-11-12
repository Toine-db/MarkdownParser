using MarkdownParser.Models;

namespace MarkdownParser.Test.Mocks;

internal class PassThroughComponentSupplier : IViewSupplier<object>
{
    public MarkdownReferenceDefinition[]? MarkdownReferenceDefinitions { get; private set; }

    public void RegisterReferenceDefinitions(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        MarkdownReferenceDefinitions = markdownReferenceDefinitions.ToArray();
    }

    public object GetTextView(TextBlock textBlock)
    {
        return textBlock;
    }

    public object GetBlockquotesView(object childView)
    {
        return childView;
    }

    public object GetHeaderView(TextBlock textBlock, int headerLevel)
    {
        return textBlock;
    }

    public object GetImageView(string url, string subscription, string imageId)
    {
        return url;
    }

    public object GetListView(List<object> items)
    {
        return items;
    }

    public object GetListItemView(object childView, bool isOrderedList, int sequenceNumber, int listLevel)
    {
        return childView;
    }

    public object GetStackLayoutView(List<object> childViews)
    {
        return childViews;
    }

    public object GetThematicBreak()
    {
        return "ThematicBreak";
    }

    public object GetPlaceholder(string placeholderName)
    {
        return placeholderName;
    }

    public object GetFencedCodeBlock(TextBlock textBlock, string codeInfo)
    {
        return textBlock;
    }

    public object GetIndentedCodeBlock(TextBlock textBlock)
    {
        return textBlock;
    }

    public object GetHtmlBlock(TextBlock textBlock)
    {
        return textBlock;
    }
}