using MarkdownParser.Models;

namespace MarkdownParser.Test.Mocks;

internal class PassThroughComponentSupplier : IViewSupplier<object>
{
    public MarkdownReferenceDefinition[]? MarkdownReferenceDefinitions { get; private set; }

    public void OnReferenceDefinitionsPublished(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        MarkdownReferenceDefinitions = markdownReferenceDefinitions.ToArray();
    }

    public object CreateTextView(TextBlock textBlock)
    {
        return textBlock;
    }

    public object CreateBlockquotesView(object childView)
    {
        return childView;
    }

    public object CreateHeaderView(TextBlock textBlock, int headerLevel)
    {
        return textBlock;
    }

    public object CreateImageView(string url, string subscription, string imageId)
    {
        return url;
    }

    public object CreateListView(List<object> items)
    {
        return items;
    }

    public object CreateListItemView(object childView, bool isOrderedList, int sequenceNumber, int listLevel)
    {
        return childView;
    }

    public object CreateStackLayoutView(List<object> childViews)
    {
        return childViews;
    }

    public object CreateThematicBreak()
    {
        return "ThematicBreak";
    }

    public object CreatePlaceholder(string placeholderName)
    {
        return placeholderName;
    }

    public object CreateFencedCodeBlock(TextBlock textBlock, string codeInfo)
    {
        return textBlock;
    }

    public object CreateIndentedCodeBlock(TextBlock textBlock)
    {
        return textBlock;
    }

    public object CreateHtmlBlock(TextBlock textBlock)
    {
        return textBlock;
    }

    public void Clear()
    {
        MarkdownReferenceDefinitions = [];
    }
}