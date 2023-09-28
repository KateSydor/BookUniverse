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

        /*public static void Fill(string mySecretValue, string insertQuery)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(mySecretValue);
            connection.Open();

            using NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection);
            command.ExecuteNonQuery();
        }*/
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
            /*string sqlScript = "INSERT INTO \"Category\" (category_id, category_name) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({category[i].Id}, \'{category[i].CategoryName}\')";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);*/
            Fill(mySecretValue, category, "Category");

            var userFaker = DataGenerator.GetUserRule();
            userFaker.UseSeed(0);
            var user = userFaker.Generate(count);
            /* sqlScript = "INSERT INTO \"User\" (user_id, username, email, password, role) VALUES ";
             for (int i = 0; i < count; i++)
             {
                 int param1 = user[i].Id;
                 string param2 = user[i].UserName;
                 string param3 = user[i].Email;
                 string param4 = user[i].Password;
                 int param5 = user[i].Role;
                 sqlScript += $"({param1}, \'{param2}\', \'{param3}\', \'{param4}\', {param5})";
                 sqlScript += (i < count - 1) ? ", " : ";";
             }
             Fill(mySecretValue, sqlScript);*/
            Fill(mySecretValue, user, "User");


            var bookFaker = DataGenerator.GetBookRule(count);
            var book = bookFaker.Generate(count);
            /*sqlScript = "INSERT INTO \"Book\" (book_id, title, author, description, number_of_pages, rating, category_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                int param1 = book[i].Id;
                string param2 = book[i].Title;
                string param3 = book[i].Author;
                string param4 = book[i].Description;
                int param5 = book[i].NumberOfPages;
                double param6 = book[i].Rating;
                int param7 = book[i].CategoryId;
                sqlScript += $"({param1}, \'{param2}\', \'{param3}\', \'{param4}\', {param5}, {param6.ToString(CultureInfo.InvariantCulture)}, {param7})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);*/
            Fill(mySecretValue, book, "Book");

            var folderFaker = DataGenerator.GetFolderRule(count);
            var folder = folderFaker.Generate(count);
            /*sqlScript = "INSERT INTO \"Folder\" (folder_id, folder_name, user_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({folder[i].Id}, \'{folder[i].FolderName}\', {folder[i].UserId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);*/
            Fill(mySecretValue, folder, "Folder");

            var userBookFaker = DataGenerator.GetUserBookRule(count);
            var userBook = userBookFaker.Generate(count);
            /*sqlScript = "INSERT INTO \"UserBook\" (id, user_id, book_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({userBook[i].Id}, {userBook[i].UserId}, {userBook[i].BookId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);*/
            Fill(mySecretValue, userBook, "UserBook");

            var favouritesFaker = DataGenerator.GetFavouritesRule(count);
            var favourites = favouritesFaker.Generate(count);
            /*sqlScript = "INSERT INTO \"Favourites\" (favorites_id, book_id, user_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({favourites[i].Id}, {favourites[i].BookId}, {favourites[i].UserId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);*/
            Fill(mySecretValue, favourites, "Favourites");

            var bookFolderFaker = DataGenerator.GetBookFolderRule(count);
            var bookFolder = bookFolderFaker.Generate(count);
            /*sqlScript = "INSERT INTO \"BookFolder\" (id, book_id, folder_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({bookFolder[i].Id}, {bookFolder[i].BookId}, {bookFolder[i].FolderId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);*/
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
            ReadAndPrintTableData(connection, "Favourites");
            ReadAndPrintTableData(connection, "BookFolder");
        }
    }
}