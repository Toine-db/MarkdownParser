using FluentAssertions;
using MarkdownParser.Test.Mocks;
using MarkdownParser.Test.Services;

namespace MarkdownParser.Test;

[TestClass]
public class MarkdownParserTextSegmentsSpecs
{
    [TestMethod]
    public void When_parsing_text_with_emphasis_it_should_output_literal_text_correct()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("TextEmphasis.basic-text.md");

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
        parseResult[0].Should().StartWith("textview:Use double tildes around the words.");
        parseResult[1].Should().StartWith("textview:Paragraphs are separated by a blank line.");
        parseResult[2].Should().StartWith("textview:3rd paragraph. this is Italic, this is bold, and monospace. Itemized lists\r\nlook like:");
    }

    [TestMethod]
    public void When_parsing_text_with_links_it_should_output_literal_text_correct()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("TextEmphasis.basic-links.md");

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
        parseResult[0].Should().StartWith("textview:My favorite search engine is Duck Duck Go.");
        parseResult[1].Should().StartWith("textview:https://www.markdownguide.org\r\nfake@example.com");
    }
}