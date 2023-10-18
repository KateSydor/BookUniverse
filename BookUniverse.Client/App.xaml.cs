using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.Client.ViewModels.Factories;
using BookUniverse.Client.ViewModels;
using BookUniverse.DAL.Persistence;
using BookUniverse.DAL.Repositories.Base;
using BookUniverse.DAL.Repositories.BookFolderRepository;
using BookUniverse.DAL.Repositories.BookRepository;
using BookUniverse.DAL.Repositories.CategoryRepository;
using BookUniverse.DAL.Repositories.FolderRepository;
using BookUniverse.DAL.Repositories.UserBookRepository;
using BookUniverse.DAL.Repositories.UserRepository;
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

                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<IBookFolderRepository, BookFolderRepository>();
                    services.AddScoped<IBookRepository, BookRepository>();
                    services.AddScoped<ICategoryRepository, CategoryRepository>();
                    services.AddScoped<IFolderRepository, FolderRepository>();
                    services.AddScoped<IUserBookRepository, UserBookRepository>();
                    services.AddScoped<IUserRepository, UserRepository>();

                    services.AddSingleton<IRootSimpleTraderViewModelFactory, RootSimpleTraderViewModelFactory>();
                    services.AddSingleton<ISimpleTraderViewModelFactory<LoginViewModel>, LoginViewModelFactory>();

                    services.AddScoped<IAuthenticator, Authenticator>();
                    services.AddScoped<IAuthenticationService, AuthenticationService>();
                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
