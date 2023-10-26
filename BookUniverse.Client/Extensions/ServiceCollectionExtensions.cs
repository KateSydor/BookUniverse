﻿namespace BookUniverse.Client.Extensions
{
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Services;
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

        public static void AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        public static void AddViews(this IServiceCollection services)
        {
            services.AddScoped<MainWindow>();
            services.AddScoped<SignInWindow>();
        }
    }
}