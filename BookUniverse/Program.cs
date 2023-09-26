using Microsoft.Extensions.Configuration;
using Bogus;
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


namespace BookUniverseConsole
{
    

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Category { get; set; }
    public DbSet<User> User { get; set; }
}



    class Program
    {

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>().Build();
            string mySecretValue = builder["ConnectionString"];
            Utils.ConnectFillDB(mySecretValue);

            /*Randomizer.Seed = new Random(54815148);

            var categoryFaker = new Faker<Category>()
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.CategoryName, f => f.PickRandom("Fantasy", "Novel", "Detective", "Poetry", "Science", "Horror"));

             var userFaker = new Faker<User>()
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password())
            .RuleFor(x => x.Role, f => f.Random.Number(1, 3)); 

            var bookFaker = new Faker<Book>()
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Author, f => f.Name.FullName())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            .RuleFor(x => x.NumberOfPages, f => f.Random.Number(50, 800))
            .RuleFor(x => x.Rating, f => f.Random.Double(1, 6))
            .RuleFor(x => x.CategoryId, f => f.Random.Number(1, 100));
            
             var folderFaker = new Faker<Folder>()
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.FolderName, f => f.Commerce.ProductName())
            .RuleFor(x => x.UserId, f => f.Random.Number(1, 100)); 

            var userBookFaker = new Faker<UserBook>()
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.UserId, f => f.Random.Number(1, 100)) 
            .RuleFor(x => x.BookId, f => f.Random.Number(1, 100)); 
            
            var favouritesFaker = new Faker<Favourites>()
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.BookId, f => f.Random.Number(1, 100)) 
            .RuleFor(x => x.UserId, f => f.Random.Number(1, 100));

            var bookFolderFaker = new Faker<BookFolder>()
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.BookId, f => f.Random.Number(1, 100)) 
            .RuleFor(x => x.FolderId, f => f.Random.Number(1, 100)); 
*/

        }
    }
}
 