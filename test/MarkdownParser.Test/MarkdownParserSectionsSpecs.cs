using System.Text.RegularExpressions;
using FluentAssertions;
using MarkdownParser.Test.Mocks;
using MarkdownParser.Test.Services;

namespace MarkdownParser.Test;

[TestClass]
public class MarkdownParserSectionsSpecs
{
    [TestMethod]
    public void When_parsing_paragraphs_it_should_output_multiple_text_views_in_good_order()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.paragraphs.md");

        var mockComponentSupplier = new StringComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(3);
        parseResult[0].Should().StartWith("textview:Paragraphs are");
        parseResult[1].Should().StartWith("textview:2nd paragraph.");
        parseResult[2].Should().StartWith("textview:Note that");
    }

    [TestMethod]
    public void When_parsing_headers_it_should_output_header_views()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.headers.md");

        var mockComponentSupplier = new StringComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(3);
        parseResult[0].Should().StartWith("headerview:1:");
        parseResult[1].Should().StartWith("headerview:2:");
        parseResult[2].Should().StartWith("headerview:3:");
    }

    [TestMethod]
    public void When_parsing_nested_list_it_should_output_nesting_by_level()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.nestedlist.md");

        var mockComponentSupplier = new StringComponentSupplier();
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
        splittedViews[3].Should().Be("item1-listitemview");
        splittedViews[4].Should().Be("_False.1.1_stackview>");
        splittedViews[5].Should().Be("+textview");
        splittedViews[6].Should().Be("item2+listview>");
        splittedViews[7].Should().Be("-listitemview");
        splittedViews[8].Should().Be("_False.2.1_textview");
        splittedViews[9].Should().Be("item2-1-listitemview");
        splittedViews[10].Should().Be("_False.2.1_textview");
        splittedViews[11].Should().Be("item2-2-listitemview");
        splittedViews[12].Should().Be("_False.2.1_textview");
        splittedViews[13].Should().Be("item2-3<listview<stackview<listview");
    }

    [TestMethod]
    public void When_parsing_codeblocks_it_should_output_code_views()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.codeblocks.md");

        var mockComponentSupplier = new StringComponentSupplier();
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
        codeViewComponentsGroup0.Length.Should().Be(4);
        codeViewComponentsGroup0[0].Should().Be("fencedcodeview>");
        codeViewComponentsGroup0[1].Should().Be("(cs)");
        codeViewComponentsGroup0[2].Should().Be("var myNumber = 1;\r\nmyNumber++;");
        codeViewComponentsGroup0[3].Should().Be("<fencedcodeview");

        var codeViewComponentsGroup1 = parseResult[1].Split('|');
        codeViewComponentsGroup1.Length.Should().Be(3);
        codeViewComponentsGroup1[0].Should().Be("indentedview>");
        codeViewComponentsGroup1[1].Should().Be("the first line for IndentedCode code block\r\nthe second line for IndentedCode code block\r\nthe third line for IndentedCode code block");
        codeViewComponentsGroup1[2].Should().Be("<indentedview");
    }
    
    [TestMethod]
    public void When_parsing_htmlblocks_it_should_output_html_views()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.htmlblocks.md");

        var mockComponentSupplier = new StringComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        var newLineIndicator = Settings.TextualLineBreak;

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(2);

        var htmlViewComponentsGroup0 = parseResult[0].Split('|');
        htmlViewComponentsGroup0.Length.Should().Be(3);
        htmlViewComponentsGroup0.First().Should().Be("htmlview>");
        htmlViewComponentsGroup0.Last().Should().Be("<htmlview");

        var firstHtmlViewContentGroup = htmlViewComponentsGroup0[1].Split(newLineIndicator);
        firstHtmlViewContentGroup.Length.Should().Be(5);
        firstHtmlViewContentGroup[0].Trim().Should().Be("<p>First text in block</p>");
        firstHtmlViewContentGroup[1].Trim().Should().Be("<div>");
        firstHtmlViewContentGroup[2].Trim().Should().Be("<h1>Header</h1>");
        firstHtmlViewContentGroup[3].Trim().Should().Be("<p>Same block but nested element</p>");
        firstHtmlViewContentGroup[4].Trim().Should().Be("</div>");

        var htmlViewComponentsGroup1 = parseResult[1].Split('|');
        htmlViewComponentsGroup1.Length.Should().Be(3);
        htmlViewComponentsGroup1.First().Should().Be("htmlview>");
        htmlViewComponentsGroup1.Last().Should().Be("<htmlview");

        var secondHtmlViewContentGroup = htmlViewComponentsGroup1[1].Split(newLineIndicator);
        secondHtmlViewContentGroup.Length.Should().Be(8);
        secondHtmlViewContentGroup[0].Trim().Should().Be("<article>");
        secondHtmlViewContentGroup[1].Trim().Should().Be("<header>");
        secondHtmlViewContentGroup[2].Trim().Should().Be("<h1>A heading</h1>");
        secondHtmlViewContentGroup[3].Trim().Should().Be("<p>Posted by John Doe</p>");
        secondHtmlViewContentGroup[4].Trim().Should().Be("<p>Some additional information here</p>");
        secondHtmlViewContentGroup[5].Trim().Should().Be("</header>");
        secondHtmlViewContentGroup[6].Trim().Should().Be("<p>Lorem Ipsum...</p>");
        secondHtmlViewContentGroup[7].Trim().Should().Be("</article>");
    }

    [TestMethod]
    public void When_parsing_reference_definitions_it_should_output_specific_views()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("Sections.referencedefinitions.md");

        var mockComponentSupplier = new StringComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult[0].Should().StartWith("textview:Aliquet in luctus in porttitor non quam donec.");
        mockComponentSupplier.MarkdownReferenceDefinitions.Should().HaveCount(1);

        mockComponentSupplier.MarkdownReferenceDefinitions[0].IsPlaceholder = false;
        mockComponentSupplier.MarkdownReferenceDefinitions[0].PlaceholderId = "PORTTITOR NON QUAM";
        mockComponentSupplier.MarkdownReferenceDefinitions[0].Title = "lipsum";
        mockComponentSupplier.MarkdownReferenceDefinitions[0].Url = "https://lipsum.com/";
    }
}