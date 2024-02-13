using BookStorage.BookManager;

namespace BookStorage.Tests.BookManager
{
    [TestFixture]
    public class FilterTests
    {
        private readonly Filter _filter;

        private string _title;
        private string _genre;
        private string _author;
        private string _publisher;
        private int _moreThanPages;
        private int _lessThanPages;
        private DateTime _publishedBefore;
        private DateTime _publishedAfter;

        [SetUp]
        public void Setup()
        {
            _title = "Test Title";
            _genre = "Test Genre";
            _author = "Test Author";
            _publisher = "Test Publisher";
            _moreThanPages = 100;
            _lessThanPages = 300;
            _publishedBefore = new DateTime(2022, 1, 1);
            _publishedAfter = new DateTime(2021, 1, 1);
        }

        [Test]
        public void FilterProperties_CanBeSetAndGet()
        {
            var filter = new Filter()
            {
                Title = _title,
                Genre = _genre,
                Author = _author,
                Publisher = _publisher,
                MoreThanPages = _moreThanPages,
                LessThanPages = _lessThanPages,
                PublishedBefore = _publishedBefore,
                PublishedAfter = _publishedAfter
            };

            Assert.Multiple(() =>
            {
                Assert.That(filter.Title, Is.EqualTo(_title));
                Assert.That(filter.Genre, Is.EqualTo(_genre));
                Assert.That(filter.Author, Is.EqualTo(_author));
                Assert.That(filter.Publisher, Is.EqualTo(_publisher));
                Assert.That(filter.MoreThanPages, Is.EqualTo(_moreThanPages));
                Assert.That(filter.LessThanPages, Is.EqualTo(_lessThanPages));
                Assert.That(filter.PublishedBefore, Is.EqualTo(_publishedBefore));
                Assert.That(filter.PublishedAfter, Is.EqualTo(_publishedAfter));
            });
        }

        [Test]
        public void FilterProperties_CanBeNull()
        {
            var filter = new Filter();

            Assert.Multiple(() =>
            {
                Assert.That(filter.Title, Is.EqualTo(null));
                Assert.That(filter.Genre, Is.EqualTo(null));
                Assert.That(filter.Author, Is.EqualTo(null));
                Assert.That(filter.Publisher, Is.EqualTo(null));
                Assert.That(filter.MoreThanPages, Is.EqualTo(null));
                Assert.That(filter.LessThanPages, Is.EqualTo(null));
                Assert.That(filter.PublishedBefore, Is.EqualTo(null));
                Assert.That(filter.PublishedAfter, Is.EqualTo(null));
            });
        }
    }
}
