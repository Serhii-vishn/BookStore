using Books.Data;
using Books.Models;

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
    }
}