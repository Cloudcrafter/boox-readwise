using Boox.NoteParser.Models;
namespace Boox.NoteParser;
public class HighLightsParser
{
    private const string PageNoIndicator = "  Page No.: ";
    private const string NoteSeparator = "-------------------";
    private const string DatePageSeparator = "  |  ";
    private const string StartIndicator = "Reading Notes |";
    private const string TitleStartIndicator = "<<";
    private const string TitleEndIndicator = ">>";
    private const string TitleSubtitleSeparator = "_ ";
    public Book GetHighlights(string[] highlightsFileContent)
    {
        var book = new Book();

        ParseNotes(highlightsFileContent, book);

        return book;
    }

    private void ParseNotes(string[] highlightsFileContent, Book book)
    {
        foreach (var highlightsFileLine in highlightsFileContent)
        {
            if (string.IsNullOrEmpty(highlightsFileLine))
            {
                continue;
            }

            if (highlightsFileLine.StartsWith(StartIndicator))
            {
                (book.Title, book.Author) = GetTitleAndAuthor(highlightsFileLine);
                continue;
            }

            if (highlightsFileLine.Contains(PageNoIndicator))
            {
                continue;
            }
        }
    }

    private (string Title, string Author) GetTitleAndAuthor(string startLine)
    {
        int startIndex = startLine.IndexOf(TitleStartIndicator) + TitleStartIndicator.Length;
        int endIndex = startLine.IndexOf(TitleEndIndicator) + TitleEndIndicator.Length;

        string[] titleSubTitle = startLine
            .Substring(startIndex, endIndex - startIndex)
            .Split(TitleSubtitleSeparator, StringSplitOptions.RemoveEmptyEntries);

        string title = titleSubTitle[0];
        string author = startLine.Substring(endIndex, startLine.Length - endIndex);

        return (title, author);
    }
}
