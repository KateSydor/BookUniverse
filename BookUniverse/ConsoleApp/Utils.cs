using Bogus;
using Bogus.DataSets;
using Npgsql;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookUniverseConsole
{
    public static class Utils
    {
        public static void ConnectFillDB(string mySecretValue)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(mySecretValue))
            {
                connection.Open();
                Console.WriteLine("Connected to PostgreSQL database");

                string sqlScript = File.ReadAllText("./SQL/script.sql");
                using (NpgsqlCommand command = new NpgsqlCommand(sqlScript, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("SQL script executed successfully");
                }
            }
        }

        public static void Fill(string mySecretValue, string insertQuery)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(mySecretValue))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void GenerateData(string mySecretValue, int count)
        {
            Randomizer.Seed = new Random(54815148);

            var categoryFaker = new Faker<Category>()
            .RuleFor(x => x.Id, f => f.IndexFaker+1)
            .RuleFor(x => x.CategoryName, f => f.PickRandom("Fantasy", "Novel", "Detective", "Poetry", "Science", "Horror"));
            var category = categoryFaker.Generate(count);
            string sqlScript = "INSERT INTO \"Category\" (category_id, category_name) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({category[i].Id}, \'{category[i].CategoryName}\')";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);

            var userFaker = new Faker<User>()
           .RuleFor(x => x.Id, f => f.IndexFaker+1)
           .RuleFor(x => x.UserName, f => f.Internet.UserName())
           .RuleFor(x => x.Email, f => f.Internet.Email())
           .RuleFor(x => x.Password, f => f.Internet.Password())
           .RuleFor(x => x.Role, f => f.Random.Number(0, 1));
            userFaker.UseSeed(0);
            var user = userFaker.Generate(count);
            sqlScript = "INSERT INTO \"User\" (user_id, username, email, password, role) VALUES ";
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
            Fill(mySecretValue, sqlScript);

            var bookFaker = new Faker<Book>()
            .RuleFor(x => x.Id, f => f.IndexFaker+1)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Author, f => f.Name.FullName())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            .RuleFor(x => x.NumberOfPages, f => f.Random.Number(50, 800))
            .RuleFor(x => x.Rating, f => f.Random.Double(1, 5))
            .RuleFor(x => x.CategoryId, f => f.Random.Number(1, count - 1));
            var book = bookFaker.Generate(count);
            sqlScript = "INSERT INTO \"Book\" (book_id, title, author, description, number_of_pages, rating, category_id) VALUES ";
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
            Fill(mySecretValue, sqlScript);

            var folderFaker = new Faker<Folder>()
           .RuleFor(x => x.Id, f => f.IndexFaker+1)
           .RuleFor(x => x.FolderName, f => f.Commerce.ProductName())
           .RuleFor(x => x.UserId, f => f.Random.Number(1, count - 1));
            var folder = folderFaker.Generate(count);
            sqlScript = "INSERT INTO \"Folder\" (folder_id, folder_name, user_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({folder[i].Id}, \'{folder[i].FolderName}\', {folder[i].UserId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);

            var userBookFaker = new Faker<UserBook>()
            .RuleFor(x => x.Id, f => f.IndexFaker+1)
            .RuleFor(x => x.UserId, f => f.Random.Number(1, count - 1))
            .RuleFor(x => x.BookId, f => f.Random.Number(1, count - 1));
            var userBook = userBookFaker.Generate(count);
            sqlScript = "INSERT INTO \"UserBook\" (id, user_id, book_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({userBook[i].Id}, {userBook[i].UserId}, {userBook[i].BookId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);

            var favouritesFaker = new Faker<Favourites>()
            .RuleFor(x => x.Id, f => f.IndexFaker+1)
            .RuleFor(x => x.BookId, f => f.Random.Number(1, count - 1))
            .RuleFor(x => x.UserId, f => f.Random.Number(1, count - 1));
            var favourites = favouritesFaker.Generate(count);
            sqlScript = "INSERT INTO \"Favourites\" (favorites_id, book_id, user_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({favourites[i].Id}, {favourites[i].BookId}, {favourites[i].UserId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);

            var bookFolderFaker = new Faker<BookFolder>()
            .RuleFor(x => x.Id, f => f.IndexFaker+1)
            .RuleFor(x => x.BookId, f => f.Random.Number(1, count - 1))
            .RuleFor(x => x.FolderId, f => f.Random.Number(1, count - 1));
            var bookFolder = bookFolderFaker.Generate(count);
            sqlScript = "INSERT INTO \"BookFolder\" (id, book_id, folder_id) VALUES ";
            for (int i = 0; i < count; i++)
            {
                sqlScript += $"({bookFolder[i].Id}, {bookFolder[i].BookId}, {bookFolder[i].FolderId})";
                sqlScript += (i < count - 1) ? ", " : ";";
            }
            Fill(mySecretValue, sqlScript);
        }
    }
}