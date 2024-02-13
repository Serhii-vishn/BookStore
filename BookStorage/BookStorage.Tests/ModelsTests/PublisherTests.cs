using Books.Models;

namespace Models.Tests
{
    [TestFixture]
    public class PublisherTests
    {
        [Test]
        public void GenreProperties_CanBeSetAndGet()
        {
            var publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Test Publisher",
            };

            Assert.Multiple(() =>
            {
                Assert.That(publisher.Name, Is.EqualTo("Test Publisher"));
                Assert.That(publisher.Id, Is.Not.EqualTo(null));
                Assert.That(publisher.Books, Is.EqualTo(null));
            });
        }
    }
}
