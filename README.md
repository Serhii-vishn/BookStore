# BookStorage

This is a simple console-based book management system implemented in C#. It offers two main functionalities:

1. **Option 1: Add Books from File**
2. **Option 2: Search Books**

## Option 1: Add Books from File

This option allows the user to provide a file path via console input, parse the file, and store the data in the database. When the program is run with the same file twice, duplicate entries are not created.

## Option 2: Search Books

This option enables book search functionality using filters. It introduces a new class called `Filter` with optional fields to specify filtering criteria. The program displays the count of books matching the query and lists the titles of those books in the console. Additionally, it saves the output to a uniquely named file (using the date/time of saving) containing a list of books matching the query in the same format as the original input file.

## Usage

1. Clone this repository to your local machine.
2. Open the project in your preferred IDE (e.g., Visual Studio).
3. Build the solution.
4. Run the application.
5. Choose one of the options presented in the console.

### Option 1: Add Books from File

- Select Option 1 from the menu.
- Enter the file path when prompted.
- The program will parse the file, store the data in the database, and notify you of any duplicate entries.

### Option 2: Search Books

- Select Option 2 from the menu.
- Enter the data for sorting.
- The program will display the count of books matching the query and list their titles in the console.
- It will also save the output to a uniquely named file containing a list of books matching the query.

## Configuration

- Database connection settings can be configured in the `appSettings.json` file.

## Dependencies

- .NET Core 3.1
- EF Core 8
- CsvHelper

## Contribution Guidelines

Contributions to the project are welcome. Follow these guidelines:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Make your changes and submit a pull request.
4. Ensure your code passes the existing unit tests.

## License

This project is licensed under the MIT License - [MIT License](LICENSE.txt).
