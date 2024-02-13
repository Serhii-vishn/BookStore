using CsvHelper.Configuration;

namespace BookStore.BookManager
{
    public sealed class CsvBookModel
    {
        public string Title { get; set; } = null!;
        public int Pages { get; set; }
        public string Genre { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Author { get; set; } = null!;
        public string Publisher { get; set; } = null!;
    }

    public sealed class CsvBookModelMap : ClassMap<CsvBookModel>
    {
        public CsvBookModelMap()
        {
            Map(m => m.Title);
            Map(m => m.Pages);
            Map(m => m.Genre);
            Map(m => m.ReleaseDate).TypeConverter<CustomDateTimeConverter>();
            Map(m => m.Author);
            Map(m => m.Publisher);
        }
    }
}
