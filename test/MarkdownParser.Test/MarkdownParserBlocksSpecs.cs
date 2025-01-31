using System.Text.RegularExpressions;
using FluentAssertions;
using MarkdownParser.Models;
using MarkdownParser.Test.Mocks;
using MarkdownParser.Test.Services;
using static System.Net.Mime.MediaTypeNames;

namespace MarkdownParser.Test;

[TestClass]
public class MarkdownParserBlocksSpecs
{
    [TestMethod]
    public void When_parsing_paragraphs_it_should_output_paragraph_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.paragraphs.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(3);
        parseResult[0].Split(':')[1].Should().Be($"[]{BlockType.Paragraph}");
        parseResult[1].Split(':')[1].Should().Be($"[]{BlockType.Paragraph}");
        parseResult[2].Split(':')[1].Should().Be($"[]{BlockType.Paragraph}");
    }

    [TestMethod]
    public void When_parsing_headers_it_should_output_header_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.headers.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(3);
        parseResult[0].Split(':')[2].Should().Be($"[]{BlockType.Heading}");
        parseResult[1].Split(':')[2].Should().Be($"[]{BlockType.Heading}");
        parseResult[2].Split(':')[2].Should().Be($"[]{BlockType.Heading}");
    }

    [TestMethod]
    public void When_parsing_nested_list_it_should_output_nesting_list_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.nestedlist.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1); // just becuase it only outputs a single string

        var listviewCount = Regex.Matches(parseResult[0], "listview>:").Cast<Match>().Count();
        listviewCount.Should().Be(2);

        var splittedViews = parseResult[0].Split(':');
        splittedViews[0].Should().Be("listview>");
        splittedViews[1].Should().Be("-listitemview");
        splittedViews[2].Should().Be("_False.1.1_textview");
        splittedViews[3].Should().Be($"[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[4].Should().Be("item1-listitemview");
        splittedViews[5].Should().Be("_False.1.1_stackview>");
        splittedViews[6].Should().Be("+textview");
        splittedViews[7].Should().Be($"[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[8].Should().Be("item2+listview>");
        splittedViews[9].Should().Be("-listitemview");
        splittedViews[10].Should().Be("_False.2.1_textview");
        splittedViews[11].Should().Be($"[]{BlockType.List}[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[12].Should().Be("item2-1-listitemview");
        splittedViews[13].Should().Be("_False.2.1_textview");
        splittedViews[14].Should().Be($"[]{BlockType.List}[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[15].Should().Be("item2-2-listitemview");
        splittedViews[16].Should().Be("_False.2.1_textview");
        splittedViews[17].Should().Be($"[]{BlockType.List}[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[18].Should().Be("item2-3<listview<stackview<listview");
    }

    [TestMethod]
    public void When_parsing_nested_list_it_should_output_nesting_list_ancestors_with_inner_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.innernestedlist.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        var splittedViews = parseResult[0].Split(':');
        splittedViews[4].Should().Be($"[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[9].Should().Be($"[]{BlockType.List}[]{BlockType.List}[]{BlockType.Heading}");
        splittedViews[12].Should().Be($"[]{BlockType.List}[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[16].Should().Be($"[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[22].Should().Be($"[]{BlockType.List}[]{BlockType.List}[]{BlockType.Quote}[]{BlockType.Paragraph}");
        splittedViews[25].Should().Be($"[]{BlockType.List}[]{BlockType.List}[]{BlockType.Quote}[]{BlockType.Heading}");
    }

    [TestMethod]
    public void When_parsing_codeblocks_it_should_output_code_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.codeblocks.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(2);

        var codeViewComponentsGroup0 = parseResult[0].Split('|');
        codeViewComponentsGroup0[0].Should().Be("fencedcodeview>");
        codeViewComponentsGroup0[3].Should().Be($"[]{BlockType.FencedCode}");

        var codeViewComponentsGroup1 = parseResult[1].Split('|');
        codeViewComponentsGroup1[0].Should().Be("indentedview>");
        codeViewComponentsGroup1[2].Should().Be($"[]{BlockType.IndentedCode}");
    }

    [TestMethod]
    public void When_parsing_codeblocks_it_should_output_code_ancestors_and_ignores_inner_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.innercodeblocks.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult[0].Should().Contain("[]", Exactly.Once());
        parseResult[0].Should().Contain($"[]{BlockType.FencedCode}");
        parseResult[1].Should().Contain("[]", Exactly.Once());
        parseResult[1].Should().Contain($"[]{BlockType.IndentedCode}");
    }

    [TestMethod]
    public void When_parsing_htmlblocks_it_should_output_html_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.htmlblocks.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        var htmlViewComponentsGroup0 = parseResult[0].Split('|');
        htmlViewComponentsGroup0.First().Should().Be("htmlview>");
        htmlViewComponentsGroup0[2].Should().Be($"[]{BlockType.Html}");

        var htmlViewComponentsGroup1 = parseResult[1].Split('|');
        htmlViewComponentsGroup1.First().Should().Be("htmlview>");
        htmlViewComponentsGroup1[2].Should().Be($"[]{BlockType.Html}");
    }

    [TestMethod]
    public void When_parsing_htmlblocks_it_should_output_html_ancestors_and_ignores_inner_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.innerhtmlblocks.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult[0].Should().Contain("[]", Exactly.Once());
        parseResult[0].Should().Contain($"[]{BlockType.Html}");
    }

    [TestMethod]
    public void When_parsing_reference_definitions_it_should_output_correct_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.referencedefinitions.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult[0].Should().StartWith($"textview:[]{BlockType.Paragraph}:Aliquet in luctus in porttitor non quam donec.");
    }

    [TestMethod]
    public void When_parsing_quotes_it_should_output_quote_ancestors_with_inner_ancestors()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.innerblockquotes.md");

        var mockComponentSupplier = new BlockComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        var splittedViews = parseResult[0].Split(':');
        splittedViews[3].Should().Be($"[]{BlockType.Quote}[]{BlockType.Paragraph}");
        splittedViews[5].Should().Be($"[]{BlockType.Quote}[]{BlockType.Paragraph}");
        splittedViews[8].Should().Be($"[]{BlockType.Quote}[]{BlockType.Heading}");
        splittedViews[10].Should().Be($"[]{BlockType.Quote}[]{BlockType.Paragraph}");
        splittedViews[14].Should().Be($"[]{BlockType.Quote}[]{BlockType.List}[]{BlockType.Paragraph}");
        splittedViews[17].Should().Be($"[]{BlockType.Quote}[]{BlockType.List}[]{BlockType.Paragraph}");
    }

}
