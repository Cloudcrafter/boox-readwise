using System.Globalization;
using Boox.NoteParser.Models;
namespace Boox.NoteParser;
public class HighLightsParser
{
    private const string PageNoIndicator = "Page No.: ";
    private const string NoteSeparator = "-------------------";
    private const string DatePageSeparator = "|";
    private const string StartIndicator = "Reading Notes | ";
    private const string TitleStartIndicator = "<<";
    private const string TitleEndIndicator = ">>";
    private const string TitleSubtitleSeparator = "_ ";
    private const string HighlightNoteIndicator = "【Note】";
    public Book GetHighlights(string[] highlightsFileContent)
    {
        var book = new Book();

        ParseNotes(highlightsFileContent, book);

        return book;
    }

    private void ParseNotes(string[] highlightsFileContent, Book book)
    {
        Highlight? highlight = null;
        PreviousItemType previousItemType = PreviousItemType.None;
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
                previousItemType = PreviousItemType.StartSection;
                continue;
            }

            if (highlightsFileLine.Contains(PageNoIndicator))
            {
                if (highlight == null)
                {
                    highlight = book.StartNewHighLight();
                }
                (highlight.HighlightDate, highlight.PageNumber) = GetNoteDateAndPageNumber(highlightsFileLine);
                previousItemType = PreviousItemType.DatePageNo;
                continue;
            }

            if (highlightsFileLine.Equals(NoteSeparator))
            {
                highlight = book.StartNewHighLight();
                previousItemType = PreviousItemType.NoteSeparator;
                continue;
            }

            if (previousItemType == PreviousItemType.NoteSeparator && !highlightsFileLine.Contains(PageNoIndicator))
            {
                highlight.ChapterTitle = highlightsFileLine.Trim();
                previousItemType = PreviousItemType.ChapterLine;
                continue;
            }

            if (previousItemType == PreviousItemType.DatePageNo)
            {
                highlight.HighlightText = highlightsFileLine.Trim();
                previousItemType = PreviousItemType.Note;
                continue;
            }

            if (highlightsFileLine.StartsWith(HighlightNoteIndicator))
            {
                highlight.Note = GetHighLightAnnotation(highlightsFileLine);
                previousItemType = PreviousItemType.NoteAnnotation;
                continue;
            }
        }
    }

    private string? GetHighLightAnnotation(string highlightsFileLine)
    {
        return new String(highlightsFileLine.Skip(HighlightNoteIndicator.Length).ToArray());
    }

    private (string Title, string Author) GetTitleAndAuthor(string startLine)
    {
        int startIndex = startLine.IndexOf(TitleStartIndicator) + TitleStartIndicator.Length;
        int endIndex = startLine.IndexOf(TitleEndIndicator);

        string[] titleSubTitle = startLine
            .Substring(startIndex, endIndex - startIndex)
            .Split(TitleSubtitleSeparator, StringSplitOptions.RemoveEmptyEntries);

        string title = titleSubTitle[0];
        string author = startLine.Substring(endIndex+TitleEndIndicator.Length, startLine.Length - endIndex-TitleEndIndicator.Length);

        return (title, author);
    }

    private (DateTime NoteTime, int PageNumber) GetNoteDateAndPageNumber(string dateAndPageLIne)
    {
        //2022-12-27 06:44  |  Page No.: 18
        string[] datePageNumberComponents = dateAndPageLIne.Split(DatePageSeparator);

        DateTime.TryParseExact(datePageNumberComponents[0].Trim(), "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var noteDate);

        int.TryParse(datePageNumberComponents[1].Remove(0, PageNoIndicator.Length), out int pageNumber);

        return (noteDate, pageNumber);
    }
}
