﻿using System.Text.RegularExpressions;
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
        parseResult[0].Should().StartWith("headerview:");
        parseResult[1].Should().StartWith("headerview:");
        parseResult[1].Should().StartWith("headerview:");
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
}
