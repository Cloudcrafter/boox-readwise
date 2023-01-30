namespace Boox.NoteParser.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public List<Highlight> Highlights { get; set; } = new List<Highlight>();

        public Highlight StartNewHighLight()
        {
            var hl = new Highlight();
            Highlights.Add(hl);
            return hl;
        }
    }
}
