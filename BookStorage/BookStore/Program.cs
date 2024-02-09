using Books.BookManagement;

namespace BookStore
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("\t\t\tConsole app for managing book data in the database");
            Console.WriteLine("\t\t------------------------------------------------------------------");

            switch (AppUI.GetMenuOption())
            {
                case '1':
                    {
                        try
                        {
                            string filePath = AppUI.GetFilePathFromUser();
                            var numberRecordsDB = BookDataProcessor.ReadFile_SaveToDB(filePath);

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
                            var filteredBooks = BookDataProcessor.GetAndSavedFilteredBooks();
                            Console.WriteLine($"Found by filter {filteredBooks.Count}:");

                            foreach ( var filteredBook in filteredBooks )
                            {
                                Console.WriteLine( filteredBook );
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