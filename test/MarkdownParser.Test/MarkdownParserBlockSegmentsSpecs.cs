using FluentAssertions;
using MarkdownParser.Models;
using MarkdownParser.Models.Segments;
using MarkdownParser.Models.Segments.Indicators;
using MarkdownParser.Test.Mocks;
using MarkdownParser.Test.Services;

namespace MarkdownParser.Test;

[TestClass]
public class MarkdownParserBlockSegmentsSpecs
{
    [TestMethod]
    public void When_parsing_text_with_emphasis_it_should_output_ordered_segments()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("TextEmphasis.basic-text.md");

        var componentSupplier = new PassThroughComponentSupplier();
        var parser = new MarkdownParser<object>(componentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(3);

        parseResult[0].Should().BeOfType<TextBlock>();
        var textBlock0 = parseResult[0] as TextBlock;
        textBlock0!.TextSegments.Length.Should().Be(5);
        textBlock0!.TextSegments[0].As<TextSegment>().Text.Should().Be("Use ");
        textBlock0!.TextSegments[1].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Strikethrough);
        textBlock0!.TextSegments[1].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock0!.TextSegments[2].As<TextSegment>().Text.Should().Be("double");
        textBlock0!.TextSegments[3].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Strikethrough);
        textBlock0!.TextSegments[3].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock0!.TextSegments[4].As<TextSegment>().Text.Should().Be(" tildes around the words.");

        parseResult[1].Should().BeOfType<TextBlock>();
        var textBlock1 = parseResult[1] as TextBlock;
        textBlock1!.TextSegments.Length.Should().Be(9);
        textBlock1!.TextSegments[0].As<TextSegment>().Text.Should().Be("Paragraphs ");
        textBlock1!.TextSegments[1].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Strong);
        textBlock1!.TextSegments[1].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock1!.TextSegments[2].As<TextSegment>().Text.Should().Be("are ");
        textBlock1!.TextSegments[3].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Italic);
        textBlock1!.TextSegments[3].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock1!.TextSegments[4].As<TextSegment>().Text.Should().Be("separated");
        textBlock1!.TextSegments[5].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Italic);
        textBlock1!.TextSegments[5].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock1!.TextSegments[6].As<TextSegment>().Text.Should().Be(" by");
        textBlock1!.TextSegments[7].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Strong);
        textBlock1!.TextSegments[7].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock1!.TextSegments[8].As<TextSegment>().Text.Should().Be(" a blank line.");

        parseResult[2].Should().BeOfType<TextBlock>();
        var textBlock2 = parseResult[2] as TextBlock;
        textBlock2!.TextSegments.Length.Should().Be(15);
        textBlock2!.TextSegments[0].As<TextSegment>().Text.Should().Be("3rd paragraph. ");
        textBlock2!.TextSegments[1].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Italic);
        textBlock2!.TextSegments[1].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock2!.TextSegments[2].As<TextSegment>().Text.Should().Be("this is Italic");
        textBlock2!.TextSegments[3].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Italic);
        textBlock2!.TextSegments[3].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock2!.TextSegments[4].As<TextSegment>().Text.Should().Be(", ");
        textBlock2!.TextSegments[5].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Strong);
        textBlock2!.TextSegments[5].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock2!.TextSegments[6].As<TextSegment>().Text.Should().Be("this is bold");
        textBlock2!.TextSegments[7].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Strong);
        textBlock2!.TextSegments[7].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock2!.TextSegments[8].As<TextSegment>().Text.Should().Be(", and ");
        textBlock2!.TextSegments[9].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Code);
        textBlock2!.TextSegments[9].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock2!.TextSegments[10].As<TextSegment>().Text.Should().Be("monospace");
        textBlock2!.TextSegments[11].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Code);
        textBlock2!.TextSegments[11].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock2!.TextSegments[12].As<TextSegment>().Text.Should().Be(". Itemized lists");
        textBlock2!.TextSegments[13].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.LineBreak);
        textBlock2!.TextSegments[14].As<TextSegment>().Text.Should().Be("look like:");
    }

    [TestMethod]
    public void When_parsing_text_with_links_it_should_output_ordered_segments()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = FileReader.ReadFile("TextEmphasis.basic-links.md");

        var componentSupplier = new PassThroughComponentSupplier();
        var parser = new MarkdownParser<object>(componentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(2);

        parseResult[0].Should().BeOfType<TextBlock>();
        var textBlock0 = parseResult[0] as TextBlock;
        textBlock0!.TextSegments.Length.Should().Be(5);
        textBlock0!.TextSegments[0].As<TextSegment>().Text.Should().Be("My favorite search engine is ");
        textBlock0!.TextSegments[1].As<LinkSegment>().Indicator.Should().Be(SegmentIndicator.Link);
        textBlock0!.TextSegments[1].As<LinkSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock0!.TextSegments[1].As<LinkSegment>().Title.Should().Be("The best search engine for privacy");
        textBlock0!.TextSegments[1].As<LinkSegment>().Url.Should().Be("https://duckduckgo.com");
        textBlock0!.TextSegments[2].As<TextSegment>().Text.Should().Be("Duck Duck Go");
        textBlock0!.TextSegments[3].As<LinkSegment>().Indicator.Should().Be(SegmentIndicator.Link);
        textBlock0!.TextSegments[3].As<LinkSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock0!.TextSegments[4].As<TextSegment>().Text.Should().Be(".");

        parseResult[1].Should().BeOfType<TextBlock>();
        var textBlock1 = parseResult[1] as TextBlock;
        textBlock1!.TextSegments.Length.Should().Be(7);
        textBlock1!.TextSegments[0].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Link);
        textBlock1!.TextSegments[0].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock1!.TextSegments[0].As<LinkSegment>().Url.Should().Be("https://www.markdownguide.org");
        textBlock1!.TextSegments[1].As<TextSegment>().Text.Should().Be("https://www.markdownguide.org");
        textBlock1!.TextSegments[2].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Link);
        textBlock1!.TextSegments[2].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
        textBlock1!.TextSegments[3].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.LineBreak);
        textBlock1!.TextSegments[4].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Link);
        textBlock1!.TextSegments[4].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.Start);
        textBlock1!.TextSegments[4].As<LinkSegment>().Url.Should().Be("mailto:fake@example.com");
        textBlock1!.TextSegments[5].As<TextSegment>().Text.Should().Be("fake@example.com");
        textBlock1!.TextSegments[6].As<IndicatorSegment>().Indicator.Should().Be(SegmentIndicator.Link);
        textBlock1!.TextSegments[6].As<IndicatorSegment>().IndicatorPosition.Should().Be(SegmentIndicatorPosition.End);
    }
}