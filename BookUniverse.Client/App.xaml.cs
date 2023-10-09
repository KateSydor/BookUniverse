using BookUniverse.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace BookUniverse.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
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

                    services.AddDbContext<DatabaseContext>(options =>
                        options.UseNpgsql(sqlConnectionString));
                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var mainWindow = new MainWindow();
           // mainWindow.Show();
        }
    }
}
