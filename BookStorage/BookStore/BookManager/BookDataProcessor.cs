using Books.Data;
using Books.Models;
using BookStore.BookManager;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using BookStore.Services;
using System.Formats.Asn1;
using BookStorage.BookManager;

namespace Books.BookManagement
{
    public class BookDataProcessor
    {
        private readonly AuthorService _authorService;
        private readonly GenreService _genreService;
        private readonly PublisherService _publisherService;
        private readonly BookContext _dbContext;

        public BookDataProcessor(AuthorService authorService, GenreService genreService, PublisherService publisherService, BookContext dbContext)
        {
            _authorService = authorService;
            _genreService = genreService;
            _publisherService = publisherService;
            _dbContext = dbContext;
        }
        public int ImoportBooksFromCsv(string filePath)
        {
            int numberRecordsDB = 0;
            ValidateFilePath(filePath);

            var booksFromFile = ReadBooksFromCSV(filePath);

            foreach (var book in booksFromFile)
            {
                if (SaveToDatabase(book, _dbContext))
                    numberRecordsDB++;
            }

            return numberRecordsDB;
        }

        public List<string> GetFilteredBooks()
        {
            var booksFilter = SetFilterParameters();

            var filteredBooks = GetFilteredBooks(_dbContext, booksFilter);

            SaveFilterBooksToCSV(filteredBooks);

            return GetFilterNameLIST(filteredBooks);
        }

        private void ValidateFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File name is empty or null");
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
            if (new FileInfo(filePath).Length == 0)
                throw new ArgumentException($"File {filePath} is empty");
        }

        private static IEnumerable<CsvBookModel> ReadBooksFromCSV(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<CsvBookModelMap>();
            var records = csv.GetRecords<CsvBookModel>().ToList();

            return records;
        }

        private bool SaveToDatabase(CsvBookModel bookFromFile, BookContext db)
        {
            if (!CheckIfDataExistsInDatabase(bookFromFile, db))
            {
                var newBook = new Book
                {
                    Title = bookFromFile.Title,
                    Pages = bookFromFile.Pages,
                    Genre = _genreService.FindOrCreateGenre(bookFromFile.Genre, db),
                    ReleaseDate = bookFromFile.ReleaseDate,
                    Author = _authorService.FindOrCreateAuthor(bookFromFile.Author, db),
                    Publisher = _publisherService.FindOrCreatePublisher(bookFromFile.Publisher, db)
                };
                db.Books.Add(newBook);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        private bool CheckIfDataExistsInDatabase(CsvBookModel bookFromFile, BookContext db)
        {
            return db.Books.Any(x => x.Title == bookFromFile.Title
                                    && x.Pages == bookFromFile.Pages
                                    && x.Genre.Name == bookFromFile.Genre
                                    && x.ReleaseDate == bookFromFile.ReleaseDate
                                    && x.Author.Name == bookFromFile.Author
                                    && x.Publisher.Name == bookFromFile.Publisher);
        }

        private Filter SetFilterParameters()
        {
            var filter = new Filter();
            Console.WriteLine("\nCan be left blank for any");
            Console.Write("Title : ");
            filter.Title = Console.ReadLine();

            Console.Write("Genre: ");
            filter.Genre = Console.ReadLine();

            Console.Write("Author: ");
            filter.Author = Console.ReadLine();

            Console.Write("Publisher: ");
            filter.Publisher = Console.ReadLine();

            Console.Write("Min pages: ");
            if (int.TryParse(Console.ReadLine(), out int intValue))
                filter.MoreThanPages = intValue;

            Console.Write("Max pages: ");
            if (int.TryParse(Console.ReadLine(), out intValue))
                filter.LessThanPages = intValue;

            Console.Write("Published Before Date: ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dateTimeValue))
                filter.PublishedBefore = dateTimeValue;

            Console.Write("Published After Date: ");
            if (DateTime.TryParse(Console.ReadLine(), out dateTimeValue))
                filter.PublishedAfter = dateTimeValue;

            return filter;
        }

        private IQueryable<Book> GetFilteredBooks(BookContext db, Filter filter)
        {
            IQueryable<Book> filtredBooks = db.Books
                                    .Include(u => u.Genre)
                                    .Include(u => u.Author)
                                    .Include(u => u.Publisher);

            if (!string.IsNullOrEmpty(filter.Title))
                filtredBooks = filtredBooks
                            .Where(p => EF.Functions.Like(p.Title!, $"{filter.Title}"));

            if (!string.IsNullOrEmpty(filter.Genre))
                filtredBooks = filtredBooks
                            .Where(p => EF.Functions.Like(p.Genre.Name!, $"{filter.Genre}"));

            if (!string.IsNullOrEmpty(filter.Author))
                filtredBooks = filtredBooks
                            .Where(p => EF.Functions.Like(p.Author.Name!, $"{filter.Author}"));

            if (!string.IsNullOrEmpty(filter.Publisher))
                filtredBooks = filtredBooks
                            .Where(p => EF.Functions.Like(p.Publisher.Name!, $"{filter.Publisher}"));

            if (filter.MoreThanPages.HasValue)
                filtredBooks = filtredBooks
                          .Where(b => b.Pages >= filter.MoreThanPages);

            if (filter.LessThanPages.HasValue)
                filtredBooks = filtredBooks
                          .Where(b => b.Pages <= filter.LessThanPages);

            if (filter.PublishedBefore.HasValue)
                filtredBooks = filtredBooks
                          .Where(b => b.ReleaseDate <= filter.PublishedBefore);

            if (filter.PublishedAfter.HasValue)
                filtredBooks = filtredBooks
                          .Where(b => b.ReleaseDate >= filter.PublishedAfter);

            return filtredBooks;
        }

        private void SaveFilterBooksToCSV(IEnumerable<Book> books)
        {
            string filename = $"Books_{DateTime.Now:yyyyMMddHHmmss}.csv";

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
            };

            using var writer = new StreamWriter(filename);
            using var csv = new CsvWriter(writer, configuration);
            csv.WriteRecords(books.Select(book => new
            {
                Title = book.Title,
                Genre = book.Genre.Name,
                Author = book.Author.Name,
                Publisher = book.Publisher.Name,
                Pages = book.Pages,
                PublishedDate = book.ReleaseDate.ToString("yyyy-MM-dd")
            }));
        }

        private List<string> GetFilterNameLIST(IEnumerable<Book> filteredBooks)
        {
            var nameBooksList = new List<string>();
            foreach (var book in filteredBooks)
            {
                nameBooksList.Add(book.Title);
            }
            return nameBooksList;
        }
    }
}
