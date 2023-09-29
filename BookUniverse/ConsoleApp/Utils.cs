using Bogus;
using BookUniverseConsole.ConsoleApp;
using Npgsql;
using System.Globalization;
using System.Text;

namespace BookUniverseConsole
{
    public static class Utils
    {
        public static void ConnectFillDB(string mySecretValue)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(mySecretValue);
            connection.Open();
            Console.WriteLine("Connected to PostgreSQL database");

            string sqlScript = File.ReadAllText("./SQL/script.sql");
            using NpgsqlCommand command = new NpgsqlCommand(sqlScript, connection);
            command.ExecuteNonQuery();
            Console.WriteLine("SQL script executed successfully");
        }

        public static void Fill<T>(string mySecretValue, List<T> items, string tableName) where T : class
        {
            using NpgsqlConnection connection = new NpgsqlConnection(mySecretValue);
            connection.Open();

            foreach (var item in items)
            {
                var propertyValues = item.GetType().GetProperties()
                    .Select(prop => prop.PropertyType == typeof(double) ?
                                     ((double)prop.GetValue(item)).ToString("F1", CultureInfo.InvariantCulture) :
                                     prop.GetValue(item))
                    .ToArray();

                string sqlScript = $"INSERT INTO \"{tableName}\" ({string.Join(", ", item.GetType().GetProperties().Select(prop => prop.Name))}) " +
                                   $"VALUES ({string.Join(", ", propertyValues.Select(val => $"'{val}'"))})";

                using NpgsqlCommand command = new NpgsqlCommand(sqlScript, connection);
                command.ExecuteNonQuery();
            }
        }

        public static void GenerateData(string mySecretValue, int count)
        {
            Randomizer.Seed = new Random(54815148);

            var categoryFaker = DataGenerator.GetCategoryRule();
            var category = categoryFaker.Generate(count);
            Fill(mySecretValue, category, "Category");

            var userFaker = DataGenerator.GetUserRule();
            userFaker.UseSeed(0);
            var user = userFaker.Generate(count);
            Fill(mySecretValue, user, "User");


            var bookFaker = DataGenerator.GetBookRule(count);
            var book = bookFaker.Generate(count);
            Fill(mySecretValue, book, "Book");

            var folderFaker = DataGenerator.GetFolderRule(count);
            var folder = folderFaker.Generate(count);
            Fill(mySecretValue, folder, "Folder");

            var userBookFaker = DataGenerator.GetUserBookRule(count);
            var userBook = userBookFaker.Generate(count);
            Fill(mySecretValue, userBook, "UserBook");

            var bookFolderFaker = DataGenerator.GetBookFolderRule(count);
            var bookFolder = bookFolderFaker.Generate(count);
            Fill(mySecretValue, bookFolder, "BookFolder");

        }

        public static void ReadAndPrintTableData(NpgsqlConnection connection, string tableName)
        {
            NpgsqlCommand? command = null;
            NpgsqlDataReader? reader = null;

            try
            {
                string query = "SELECT * FROM \"" + tableName + "\"";
                command = new NpgsqlCommand(query, connection);
                reader = command.ExecuteReader();

                int separatorLength = 29;
                Console.WriteLine($"Data from the \"{tableName}\" table:");
                Console.WriteLine(new string('=', separatorLength));

                StringBuilder resultBuilder = new StringBuilder();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        string columnValue = reader[i].ToString();

                        resultBuilder.AppendLine($"{columnName}: {columnValue}");
                    }

                    resultBuilder.AppendLine(new string('-', separatorLength));
                }

                Console.WriteLine(resultBuilder.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data from table {tableName}: {ex.Message}");
            }
            finally
            {
                reader?.Close();
                command?.Dispose();
            }
        }


        public static void ReadAndPrintData(string mySecretValue)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(mySecretValue);
            connection.Open();
            Console.WriteLine("Connected to PostgreSQL database");

            ReadAndPrintTableData(connection, "Category");
            ReadAndPrintTableData(connection, "User");
            ReadAndPrintTableData(connection, "Book");
            ReadAndPrintTableData(connection, "Folder");
            ReadAndPrintTableData(connection, "UserBook");
            ReadAndPrintTableData(connection, "BookFolder");
        }
    }
}