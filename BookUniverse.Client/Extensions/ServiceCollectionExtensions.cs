namespace BookUniverse.Client.Extensions
{
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Services;
    using BookUniverse.Client.ViewModels;
    using BookUniverse.Client.ViewModels.Factories;
    using BookUniverse.Client.ViewModels.Factories.Interfaces;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;
    using BookUniverse.DAL.Repositories.BookFolderRepository;
    using BookUniverse.DAL.Repositories.BookRepository;
    using BookUniverse.DAL.Repositories.CategoryRepository;
    using BookUniverse.DAL.Repositories.FolderRepository;
    using BookUniverse.DAL.Repositories.UserBookRepository;
    using BookUniverse.DAL.Repositories.UserRepository;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseContext(this IServiceCollection services, string sqlConnectionString)
        {
            services.AddDbContext<DatabaseContext>(options =>
                    options.UseNpgsql(sqlConnectionString));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBookFolderRepository, BookFolderRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IUserBookRepository, UserBookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddViewModelFactories(this IServiceCollection services)
        {
            services.AddSingleton<IRootViewModelFactory, RootViewModelFactory>();
            services.AddSingleton<IBaseViewModelFactory<LoginViewModel>, LoginViewModelFactory>();
        }

        public static void AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticator, Authenticator>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
