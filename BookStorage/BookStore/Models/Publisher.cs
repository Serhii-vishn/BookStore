namespace Books.Models
{
    public class Publisher
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Book> Books { get; set; }
    }
}
