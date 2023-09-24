using Npgsql;

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
    }
}
