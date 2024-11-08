using MarkdownParser.Models;

namespace MarkdownParser.Test.Mocks;

internal class StringComponentSupplier : IViewSupplier<string>
{
    public MarkdownReferenceDefinition[]? MarkdownReferenceDefinitions { get; private set; }

    public void RegisterReferenceDefinitions(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        MarkdownReferenceDefinitions = markdownReferenceDefinitions.ToArray();
    }

    public string GetTextView(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"textview:{content}";
    }

    public string GetBlockquotesView(string content)
    {
        return $"blockquoteview>:{content}<blockquoteview";
    }

    public string GetHeaderView(TextBlock textBlock, int headerLevel)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"headerview:{headerLevel}:{content}";
    }

    public string GetImageView(string url, string subscription, string imageId)
    {
        return $"imageview:{url}:{subscription}";
    }

    public string GetListView(List<string> items)
    {
        // Each item will start with a '-'
        var listItems = items.Aggregate(string.Empty, (current, item) => current + $"-{item}");

        return $"listview>:{listItems}<listview";
    }

    public string GetListItemView(string content, bool isOrderedList, int sequenceNumber, int listLevel)
    {
        return $"listitemview:_{isOrderedList}.{listLevel}.{sequenceNumber}_{content}";
    }

    public string GetStackLayoutView(List<string> childViews)
    {
        var listItems = childViews.Aggregate(string.Empty, (current, item) => current + $"+{item}");

        return $"stackview>:{listItems}<stackview";
    }

    public string GetThematicBreak()
    {
        return "thematicbreakview";
    }

    public string GetPlaceholder(string placeholderName)
    {
        return $"placeholderview:{placeholderName}";
    }

    public string GetFencedCodeBlock(TextBlock textBlock, string codeInfo)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"fencedcodeview>|({codeInfo})|{content}|<fencedcodeview";
    }

    public string GetIndentedCodeBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"indentedview>|{content}|<indentedview";
    }

    public string GetHtmlBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"htmlview>|{content}|<htmlview";
    }

    public string GetReferenceDefinitions()
    {
        var content = "referencedefinitions>";
        foreach (var markdownReferenceDefinition in MarkdownReferenceDefinitions)
        {
            content += $"|{markdownReferenceDefinition.IsPlaceholder}";
            content += $"*{markdownReferenceDefinition.Label}";
            content += $"*{markdownReferenceDefinition.Title}";
            content += $"*{markdownReferenceDefinition.Url}";
        }

        content += "|<referencedefinitions";

        return content;
    }
}
