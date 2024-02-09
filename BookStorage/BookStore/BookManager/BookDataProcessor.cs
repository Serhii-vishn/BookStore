using Books.Data;
using Books.Models;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Books.BookManagement
{
    public static class BookDataProcessor
    {
        private sealed class FileBook
        {
            public string Title { get; set; } = null!;
            public int Pages { get; set; }
            public string Genre { get; set; } = null!;
            public string Author { get; set; } = null!;
            public string Publisher { get; set; } = null!;
            public DateTime ReleaseDate { get; set; }
        }

        public static uint ReadFile_SaveToDB(string filePath)
        {
            uint numberRecordsDB = 0;
            ValidateFilePath(filePath);

            var linesFromFile = File.ReadAllLines(filePath);

            using var db = new BooksContext();
            foreach (var line in linesFromFile)
            {
                string[] dataLine = line.Split(',');
                if (int.TryParse(dataLine[1], out _) && DateTime.TryParse(dataLine[3], out _))
                {
                    var bookFromFile = new FileBook()
                    {
                        Title = dataLine[0],
                        Pages = int.Parse(dataLine[1]),
                        Genre = dataLine[2],
                        ReleaseDate = DateTime.Parse(dataLine[3]),
                        Author = dataLine[4],
                        Publisher = dataLine[5]
                    };

                    if(SaveToDatabase(bookFromFile, db))
                        numberRecordsDB++;
                }
            }
            return numberRecordsDB;
        }

        public static List<string> GetAndSavedFilteredBooks()
        {
            using var db = new BooksContext();
            var booksFilter = SetFilterParameters();

            var filteredBooks = GetFilteredBooks(db, booksFilter);

            SaveFilterBooksToCSV(filteredBooks);

            return GetFilterNameLIST(filteredBooks);
        }

        private static void ValidateFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File name is empty or null");
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
            if (new FileInfo(filePath).Length == 0)
                throw new ArgumentException($"File {filePath} is empty");
        }

        private static bool SaveToDatabase(FileBook bookFromFile, BooksContext db)
        {
            if (!CheckIfDataExistsInDatabase(bookFromFile, db))
            {
                var newBook = new Book
                {
                    Title = bookFromFile.Title,
                    Pages = bookFromFile.Pages,
                    Genre = FindOrCreateGenre(bookFromFile.Genre, db),
                    ReleaseDate = bookFromFile.ReleaseDate,
                    Author = FindOrCreateAuthor(bookFromFile.Author, db),
                    Publisher = FindOrCreatePublisher(bookFromFile.Publisher, db)
                };
                db.Books.Add(newBook);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        private static bool CheckIfDataExistsInDatabase(FileBook bookFromFile, BooksContext db)
        {
            return db.Books.Any(x => x.Title == bookFromFile.Title
                                    && x.Pages == bookFromFile.Pages
                                    && x.Genre.Name == bookFromFile.Genre
                                    && x.ReleaseDate == bookFromFile.ReleaseDate
                                    && x.Author.Name == bookFromFile.Author
                                    && x.Publisher.Name == bookFromFile.Publisher);
        }

        private static Genre FindOrCreateGenre(string name, BooksContext db)
        {
            var existingGenre = db.Genres.FirstOrDefault(a => a.Name == name);

            if (existingGenre != null)
            {
                return existingGenre;
            }
            else
            {
                var newGenre = new Genre { Name = name };
                db.Genres.Add(newGenre);
                db.SaveChanges();

                return newGenre;
            }
        }

        private static Author FindOrCreateAuthor(string name, BooksContext db)
        {
            var existingAuthor = db.Authors.FirstOrDefault(a => a.Name == name);

            if (existingAuthor != null)
            {
                return existingAuthor;
            }
            else
            {
                var newAuthor = new Author { Name = name };
                db.Authors.Add(newAuthor);
                db.SaveChanges();

                return newAuthor;
            }
        }

        private static Publisher FindOrCreatePublisher(string name, BooksContext db)
        {
            var existingPublisher = db.Publishers.FirstOrDefault(a => a.Name == name);

            if (existingPublisher != null)
            {
                return existingPublisher;
            }
            else
            {
                var newPublisher = new Publisher { Name = name };
                db.Publishers.Add(newPublisher);
                db.SaveChanges();

                return newPublisher;
            }
        }

        private static Filter SetFilterParameters()
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

        private static IQueryable<Book> GetFilteredBooks(BooksContext db, Filter filter)
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

        private static void SaveFilterBooksToCSV(IEnumerable<Book> books)
        {
            string filename = $"Books_{DateTime.Now:yyyyMMddHHmmss}.csv";

            var csv = new StringBuilder();
            csv.AppendLine("Title, Genre, Author, Publisher, Pages, PublishedDate");

            foreach (var book in books)
            {
                csv.AppendLine($"{EscapeCsvField(book.Title)}, {EscapeCsvField(book.Genre.Name)}, " +
                                    $"{EscapeCsvField(book.Author.Name)}, {EscapeCsvField(book.Publisher.Name)}, " +
                                        $"{book.Pages}, {book.ReleaseDate:yyyy-MM-dd}");
            }

            File.WriteAllText(filename, csv.ToString());
        }

        private static string EscapeCsvField(string field)
        {
            if (field == null)
                return "";

            if (field.Contains("\""))
                field = field.Replace("\"", "\"\"");

            if (field.Contains(",") || field.Contains("\r") || field.Contains("\n"))
                field = $"\"{field}\"";

            return field;
        }

        private static List<string> GetFilterNameLIST(IEnumerable<Book> filteredBooks)
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