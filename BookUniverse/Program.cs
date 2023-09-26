using Microsoft.Extensions.Configuration;
using Bogus;
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace BookUniverseConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>().Build();
            string mySecretValue = builder["ConnectionString"];
            Utils.ConnectFillDB(mySecretValue);
            Utils.GenerateData(mySecretValue, 3);
            ReadAndPrintData(mySecretValue);
        }


        public static void ReadAndPrintData(string mySecretValue)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(mySecretValue))
            {
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

        private static void ReadAndPrintTableData(NpgsqlConnection connection, string tableName)
        {
            string query = $"SELECT * FROM \"{tableName}\"";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    int count =29;
                     Console.WriteLine($"Data from the \"{tableName}\" table:");
                    Console.WriteLine(new string('=', count));

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            string columnValue = reader[i].ToString();

                            Console.WriteLine($"{columnName}: {columnValue}");
                        }

                        Console.WriteLine(new string('-', count));
                    }

                    Console.WriteLine();
                }
            }
        }

    }
}
 