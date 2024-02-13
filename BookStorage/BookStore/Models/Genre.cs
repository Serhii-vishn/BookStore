namespace Books.Models
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Book> Books { get; set; }
    }
}
