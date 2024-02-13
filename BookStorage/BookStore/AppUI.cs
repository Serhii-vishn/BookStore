using Books.Data;
using Books.Models;

namespace BookStore
{
    public static class AppUI
    {
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

        public static string GetFilePathFromUser()
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
}
