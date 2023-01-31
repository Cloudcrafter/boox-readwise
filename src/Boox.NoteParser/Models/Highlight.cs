namespace Boox.NoteParser.Models
{
    public class Highlight
    {
        public string HighlightText { get; set; }
        public int PageNumber { get; set; }
        public DateTime HighlightDate { get; set; }
        public string ChapterTitle { get; set; }
        public string? Note { get; set; }
    }
}
