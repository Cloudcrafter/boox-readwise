using System.Text.Json.Serialization;

namespace Boox.ReadwiseApi.Domain.Models
{
    public class ReadwiseHighlight
    {
        public string Text { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Image_Url { get; set; }
        public string Category => "books";
        public string Location_type => "page";
        public int Location { get; set; }
        public DateTime? Highlighted_At { get; set; }
    }
}