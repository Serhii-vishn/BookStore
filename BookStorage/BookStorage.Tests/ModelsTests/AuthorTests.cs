using Books.Models;
namespace Models.Tests
{
    [TestFixture]
    public class AuthorTests
    {
        [Test]
        public void AuthorProperties_CanBeSetAndGet()
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Test Author",
            };

            Assert.Multiple(() =>
            {
                Assert.That(author.Name, Is.EqualTo("Test Author"));
                Assert.That(author.Id, Is.Not.EqualTo(null));
                Assert.That(author.Books, Is.EqualTo(null));
            });
        }
    }
}
