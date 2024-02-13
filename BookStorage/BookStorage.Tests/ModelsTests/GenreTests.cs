using Books.Models;

namespace Models.Tests
{
    [TestFixture]
    public class GenreTests
    {
        [Test]
        public void GenreProperties_CanBeSetAndGet()
        {
            var genre = new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Test Genre",
            };

            Assert.Multiple(() =>
            {
                Assert.That(genre.Name, Is.EqualTo("Test Genre"));
                Assert.That(genre.Id, Is.Not.EqualTo(null));
                Assert.That(genre.Books, Is.EqualTo(null));
            });
        }
    }
}
