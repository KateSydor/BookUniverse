namespace BookUniverse.Client
{
    using System.Windows;
    using BookUniverse.Client.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = new ConfigurationBuilder()
                        .AddUserSecrets<App>()
                        .Build();

                    string sqlConnectionString = configuration["ConnectionString"];

                    services.AddDatabaseContext(sqlConnectionString);

                    services.AddRepositories();

                    services.AddViewModelFactories();

                    services.AddAuthenticationServices();
                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e) => base.OnStartup(e);
    }
}
