using MarkdownParser.Models;
using Microsoft.VisualBasic;

namespace MarkdownParser.Test.Mocks;

internal class BlockComponentSupplier : IViewSupplier<string>
{
    public MarkdownReferenceDefinition[]? MarkdownReferenceDefinitions { get; private set; }

    public void OnReferenceDefinitionsPublished(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        MarkdownReferenceDefinitions = markdownReferenceDefinitions.ToArray();
    }

    private string AncestorsTreeToString(BlockType[] textBlockParentBlockTypeTree)
    {
        return textBlockParentBlockTypeTree.Aggregate(string.Empty, (current, blockType) => current + $"[]{blockType}");
    }

    public string CreateTextView(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"textview:{AncestorsTreeToString(textBlock.AncestorsTree)}:{content}";
    }

    public string CreateBlockquotesView(string content)
    {
        return $"blockquoteview>:{content}<blockquoteview";
    }

    public string CreateHeaderView(TextBlock textBlock, int headerLevel)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"headerview:{headerLevel}:{AncestorsTreeToString(textBlock.AncestorsTree)}:{content}";
    }

    public string CreateImageView(string url, string subscription, string imageId)
    {
        return $"imageview:{url}:{subscription}";
    }

    public string CreateListView(List<string> items)
    {
        // Each item will start with a '-'
        var listItems = items.Aggregate(string.Empty, (current, item) => current + $"-{item}");

        return $"listview>:{listItems}<listview";
    }

    public string CreateListItemView(string content, bool isOrderedList, int sequenceNumber, int listLevel)
    {
        return $"listitemview:_{isOrderedList}.{listLevel}.{sequenceNumber}_{content}";
    }

    public string CreateStackLayoutView(List<string> childViews)
    {
        var listItems = childViews.Aggregate(string.Empty, (current, item) => current + $"+{item}");

        return $"stackview>:{listItems}<stackview";
    }

    public string CreateThematicBreak()
    {
        return "thematicbreakview";
    }

    public string CreateFencedCodeBlock(TextBlock textBlock, string codeInfo)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"fencedcodeview>|({codeInfo})|{content}|{AncestorsTreeToString(textBlock.AncestorsTree)}|<fencedcodeview";
    }

    public string CreateIndentedCodeBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"indentedview>|{content}|{AncestorsTreeToString(textBlock.AncestorsTree)}|<indentedview";
    }

    public string CreateHtmlBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Settings.TextualLineBreak);
        return $"htmlview>|{content}|{AncestorsTreeToString(textBlock.AncestorsTree)}|<htmlview";
    }

    public string CreateReferenceDefinitions()
    {
        var content = "referencedefinitions>";
        foreach (var markdownReferenceDefinition in MarkdownReferenceDefinitions)
        {
            content += $"|{markdownReferenceDefinition.IsPlaceholder}";
            content += $"*{markdownReferenceDefinition.PlaceholderId}";
            content += $"*{markdownReferenceDefinition.Title}";
            content += $"*{markdownReferenceDefinition.Url}";
        }

        content += "|<referencedefinitions";

        return content;
    }

    public void Clear()
    {
        MarkdownReferenceDefinitions = [];
    }
}