namespace Boox.NoteParser.Models
{
    public class Highlight
    {
        public string HighlightText { get; set; }
        public int PageNumber { get; set; }
        public DateTime HighlightDate { get; set; }
        private string ChapterTitle { get; set; }
    }
}
