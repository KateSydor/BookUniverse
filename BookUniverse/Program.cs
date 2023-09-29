using Microsoft.Extensions.Configuration;


namespace BookUniverseConsole
{
    class Program
    {
        const int NUMBER_TO_GENERATE = 6;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>().Build();
            string mySecretValue = builder["ConnectionString"];
            Utils.ConnectFillDB(mySecretValue);
            Utils.GenerateData(mySecretValue, NUMBER_TO_GENERATE);
            Utils.ReadAndPrintData(mySecretValue);
        }
    }
}
 