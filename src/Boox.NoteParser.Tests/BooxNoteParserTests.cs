using FluentAssertions;

namespace Boox.NoteParser.Tests;

public class BooxNoteParserTests
{

    public BooxNoteParserTests()
    {

    }

    [Fact]
    public void GetHighlights_TitleAndAuthorPresentInFirstLine_TitleAndAuthorSet()
    {
        string firstLine = "Reading Notes | <<My test book_ SubtTitle - Author>>Author";
        var parser = new HighLightsParser();

        var book = parser.GetHighlights(new[] { firstLine });

        book.Author.Should().Be("Author");
        book.Title.Should().Be("My test book");

    }

    [Fact]
    public void GetHighLights_NoteDateAndPageNumberPresent_NoteDateAndPageNumberSetOnHighlight()
    {
        string firstLine = "2022-12-27 06:44  |  Page No.: 18";
        var parser = new HighLightsParser();

        var book = parser.GetHighlights(new[] { firstLine });

        book.Highlights.Count.Should().Be(1);
        book.Highlights.First().PageNumber.Should().Be(18);
        book.Highlights.First().HighlightDate.Should().Be(new DateTime(2022, 12, 27, 6, 44, 0));

    }
}