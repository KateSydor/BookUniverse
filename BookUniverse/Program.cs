using Microsoft.Extensions.Configuration;

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
        }
    }
}