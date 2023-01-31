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
        string firstLine = "Reading Notes | <<My test book_ subtitle>>Author";
        var parser = new HighLightsParser();

        var book = parser.GetHighlights(new[] { firstLine });

        book.Author.Should().Be("Author");
        book.Title.Should().Be("My test book");

    }

    [Fact]
    public void GetHighLights_NoteDateAndPageNumberPresent_NoteDateAndPageNumberSetOnHighlight()
    {
        string firstLine = "2022-12-27 06:43 | Page No.: 16";
        var parser = new HighLightsParser();

        var book = parser.GetHighlights(new[] { firstLine });

        book.Highlights.Count.Should().Be(1);
        book.Highlights.First().PageNumber.Should().Be(16);
        book.Highlights.First().HighlightDate.Should().Be(new DateTime(2022, 12, 27, 6, 43, 0));
    }

    [Fact]
    public void GetHighLights_NoteSeparatorPresent_NewHighLightIsStarted()
    {
        var lines = new[]
        {
            "Reading Notes | <<My test book_ SubtTitle - Author>>Author",
            "2022-12-27 06:43 | Page No.: 16",
            "Note text",
            "-------------------"
        };

        var parser = new HighLightsParser();
        var book = parser.GetHighlights(lines);
        book.Highlights.Count.Should().Be(2);
    }

    [Fact]
    public void GetHighLights_ChapterTitlePresent_ChapterTitleIsSet()
    {
        var lines = new[]
        {
            "Reading Notes | <<My test book_ SubtTitle - Author>>Author",
            "2022-12-27 06:43 | Page No.: 16",
            "Note text",
            "-------------------",
            "My Chapter"
        };

        var parser = new HighLightsParser();
        var book = parser.GetHighlights(lines);
        book.Highlights.Count.Should().Be(2);
        book.Highlights.Last().ChapterTitle.Should().Be("My Chapter");
    }

    [Fact]
    public void GetHighLights_NotePresent_HighLightTextIsSet()
    {
        var lines = new[]
        {
            "Reading Notes | <<My test book_ SubtTitle - Author>>Author",
            "2022-12-27 06:43 | Page No.: 16",
            "Note text",
            "-------------------",
            "My Chapter"
        };

        var parser = new HighLightsParser();
        var book = parser.GetHighlights(lines);
        book.Highlights.Count.Should().Be(2);
        book.Highlights.First().HighlightText.Should().Be(lines[2]);
    }

    [Fact]
    public void GetHighLights_NoteAnnotationPresent_NoteIsSet()
    {
        var lines = new[]
        {
            "Summary",
            "2022-12-27 06:43 | Page No.: 16",
            "Figure 1.14: How to choose the right SDLC model",
            "【Note】figurative in book to notes"
        };

        var parser = new HighLightsParser();
        var book = parser.GetHighlights(lines);
        book.Highlights.Count.Should().Be(1);
        book.Highlights.First().Note.Should().Be("figurative in book to notes");

    }
}