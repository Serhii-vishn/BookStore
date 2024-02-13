using Books.BookManagement;
using Books.Data;
using BookStore.Services;

namespace BookStore
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("\t\t\tConsole app for managing book data in the database");//TODO add console insert data to db
            Console.WriteLine("\t\t------------------------------------------------------------------");

            switch (AppUI.GetMenuOption())
            {
                case '1':
                    {
                        try
                        {
                            var dbContext = new BookContext();

                            var authorService = new AuthorService();
                            var genreService = new GenreService();
                            var publisherService = new PublisherService();

                            BookDataProcessor bookDataProcessor = new(authorService, genreService, publisherService, dbContext);
                            string filePath = AppUI.GetFilePathFromUser();
                            var numberRecordsDB = bookDataProcessor.ImoportBooksFromCsv(filePath);

                            Console.WriteLine($"Entries were made to the database: {numberRecordsDB}");
                        }
                        catch (FileNotFoundException ex)
                        {
                            Console.WriteLine("File not found: " + ex.Message);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                        }
                        break;
                    }
                case '2':
                    {
                        try
                        {
                            var dbContext = new BookContext();

                            var authorService = new AuthorService();
                            var genreService = new GenreService();
                            var publisherService = new PublisherService();

                            BookDataProcessor bookDataProcessor = new(authorService, genreService, publisherService, dbContext);
                            var filteredBooks = bookDataProcessor.GetFilteredBooks();
                            Console.WriteLine($"Found by filter {filteredBooks.Count}:");

                            foreach (var filteredBook in filteredBooks)
                            {
                                Console.WriteLine(filteredBook);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Ivalid data");
                        break;
                    }
            }
            Console.ReadLine();
        }
    }
}
