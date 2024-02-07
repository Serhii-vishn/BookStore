using Books.BookManagement;

internal class Program
{
    public static void Main()
    {
        Console.WriteLine("\t\t\tConsole app for managing book data in the database");
        Console.WriteLine("\t\t------------------------------------------------------------------");

        switch (GetMenuOption())
        {
            case '1':
                {
                    try
                    {
                        string filePath = GetFilePathFromUser();
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

    public static char GetMenuOption()
    {
        char menuOption;
        do
        {
            Console.WriteLine("\nAdd Books from File - 1");
            Console.WriteLine("Search Books - 2");

            Console.Write("Enter: ");
            menuOption = Console.ReadKey().KeyChar;
        } while (!menuOption.Equals('1') && !menuOption.Equals('2'));

        return menuOption;
    }

    private static string GetFilePathFromUser()
    {
        string filePath;
        do
        {
            Console.Write("\nEnter file path: ");
            filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: The entered file path is invalid or doesn't exist.");
            }
        } while (!File.Exists(filePath));

        return filePath;
    }
}