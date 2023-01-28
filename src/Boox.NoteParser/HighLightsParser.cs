using System.Globalization;
using Boox.NoteParser.Models;
namespace Boox.NoteParser;
public class HighLightsParser
{
    private const string PageNoIndicator = "Page No.: ";
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
        Highlight? highlight = null;
        foreach (var highlightsFileLine in highlightsFileContent)
        {
            if (string.IsNullOrEmpty(highlightsFileLine))
            {
                continue;
            }

            if (highlightsFileLine.StartsWith(StartIndicator))
            {
                (book.Title, book.Author) = GetTitleAndAuthor(highlightsFileLine);
                highlight = book.StartNewHighLight();
                continue;
            }

            if (highlightsFileLine.Contains(PageNoIndicator))
            {
                if(highlight == null)
                {
                    highlight = book.StartNewHighLight();
                }
                (highlight.HighlightDate, highlight.PageNumber) = GetNoteDateAndPageNumber(highlightsFileLine);
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

    private (DateTime NoteTime, int PageNumber) GetNoteDateAndPageNumber(string dateAndPageLIne)
    {
        //2022-12-27 06:44  |  Page No.: 18
        string[] datePageNumberComponents = dateAndPageLIne.Split(DatePageSeparator);

        DateTime.TryParseExact(datePageNumberComponents[0], "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture,DateTimeStyles.None,out var noteDate);

        int.TryParse(datePageNumberComponents[1].Remove(0,PageNoIndicator.Length), out int pageNumber);

        return (noteDate, pageNumber);
    }

}
