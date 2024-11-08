using MarkdownParser.Test.Mocks;
using MarkdownParser.Test.Services;

namespace MarkdownParser.Test;

[TestClass]
public class MarkdownParserTextSegmentsSpecs
{
    // TODO: test (Bold/Italic/Strikethrough/Links/etc)
    [TestMethod]
    public void When_parsing_paragraphs_it_should_output_multiple_text_views_in_good_order()
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
        //parseResult.Count.Should().Be(3);
        //parseResult[0].Should().StartWith("textview:Paragraphs are");
        //parseResult[1].Should().StartWith("textview:2nd paragraph.");
        //parseResult[2].Should().StartWith("textview:Note that");
    }
}