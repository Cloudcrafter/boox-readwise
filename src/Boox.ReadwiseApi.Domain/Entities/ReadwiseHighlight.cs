namespace Boox.ReadwiseApi.Domain.Models
{
    public class ReadwiseHighlight
    {
        public string Text { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? BookCoverUrl { get; set; }
        public string Category => "books";
        public string LocationType => "page";
        public int Location { get; set; }
        public string? Highlighted_At => NoteHighLight?.ToString("0") ?? null;

        public DateTime? NoteHighLight { get; set; }
    }
}