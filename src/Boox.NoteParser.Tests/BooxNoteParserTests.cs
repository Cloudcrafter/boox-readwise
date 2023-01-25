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
}