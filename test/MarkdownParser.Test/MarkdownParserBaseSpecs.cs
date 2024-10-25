using FluentAssertions;
using MarkdownParser.Test.Mocks;

namespace MarkdownParser.Test;

[TestClass]
public class MarkdownParserBaseSpecs
{
    [TestMethod]
    public void When_starting_a_parse_it_should_minimal_output_one_item()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = "hello";

        var mockComponentSupplier = new StringComponentSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().BeGreaterThan(0);
    }
}
