using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int Pages { get; set; }

        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        [ForeignKey("Author")]
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        [ForeignKey("Publisher")]
        public Guid PublisherId { get; set; }
        public Publisher Publisher { get; set; } = null!;
    }
}
