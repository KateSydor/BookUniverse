using Microsoft.Extensions.Configuration;
using Bogus;
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


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
        }
    }
}
 