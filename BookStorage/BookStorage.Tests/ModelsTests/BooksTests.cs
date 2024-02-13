using Books.Models;

namespace Models.Tests
{
    [TestFixture]
    public class BookTests
    {
        private Book _book;
        private DateTime _releaseDate;

        [SetUp]
        public void Setup()
        {
            _releaseDate = DateTime.Now;
            _book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Pages = 200,
                GenreId = Guid.NewGuid(),
                Genre = new Genre { Id = Guid.NewGuid(), Name = "Test Genre" },
                ReleaseDate = _releaseDate,
                AuthorId = Guid.NewGuid(),
                Author = new Author { Id = Guid.NewGuid(), Name = "Test Author" },
                PublisherId = Guid.NewGuid(),
                Publisher = new Publisher { Id = Guid.NewGuid(), Name = "Test Publisher" }
            };
        }

        [Test]
        public void BookProperties_NotNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_book.Id, Is.Not.EqualTo(null));
                Assert.That(_book.Pages, Is.Not.EqualTo(null));
                Assert.That(_book.GenreId, Is.Not.EqualTo(null));
                Assert.That(_book.Genre, Is.Not.EqualTo(null));
                Assert.That(_book.ReleaseDate, Is.Not.EqualTo(null));
                Assert.That(_book.AuthorId, Is.Not.EqualTo(null));
                Assert.That(_book.Author, Is.Not.EqualTo(null));
                Assert.That(_book.PublisherId, Is.Not.EqualTo(null));
                Assert.That(_book.Publisher, Is.Not.EqualTo(null));
            });
        }

        [Test]
        public void BookReleaseDate_Valid()
        {
            Assert.That(_book.ReleaseDate, Is.EqualTo(_releaseDate));
        }
    }
}
