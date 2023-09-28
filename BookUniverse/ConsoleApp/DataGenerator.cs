using Bogus;

namespace BookUniverseConsole.ConsoleApp
{
    public static class DataGenerator
    {
        public static Faker<Category> GetCategoryRule()
        {
            return new Faker<Category>()
             .RuleFor(x => x.Category_id, f => f.IndexFaker + 1)
             .RuleFor(x => x.Category_name, f => f.PickRandom("Fantasy", "Novel", "Detective", "Poetry", "Science", "Horror"));
        }

        public static Faker<User> GetUserRule()
        {
            return new Faker<User>()
             .RuleFor(x => x.User_id, f => f.IndexFaker + 1)
             .RuleFor(x => x.Username, f => f.Internet.UserName())
             .RuleFor(x => x.Email, f => f.Internet.Email())
             .RuleFor(x => x.Password, f => f.Internet.Password())
             .RuleFor(x => x.Role, f => f.Random.Number(0, 1));
        }

        public static Faker<Book> GetBookRule(int count)
        {
            return new Faker<Book>()
            .RuleFor(x => x.Book_id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Author, f => f.Name.FullName())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Number_of_pages, f => f.Random.Number(50, 800))
            .RuleFor(x => x.Rating, f => f.Random.Double(1, 5))
            .RuleFor(x => x.Category_id, f => f.Random.Number(1, count - 1));
        }

        public static Faker<Folder> GetFolderRule(int count)
        {
            return new Faker<Folder>()
             .RuleFor(x => x.Folder_id, f => f.IndexFaker + 1)
             .RuleFor(x => x.Folder_name, f => f.Commerce.ProductName())
             .RuleFor(x => x.User_id, f => f.Random.Number(1, count - 1));
        }

        public static Faker<UserBook> GetUserBookRule(int count)
        {
            return new Faker<UserBook>()
             .RuleFor(x => x.Id, f => f.IndexFaker + 1)
             .RuleFor(x => x.User_id, f => f.Random.Number(1, count - 1))
             .RuleFor(x => x.Book_id, f => f.Random.Number(1, count - 1));
        }

        public static Faker<Favourites> GetFavouritesRule(int count)
        {
            return new Faker<Favourites>()
             .RuleFor(x => x.Favorites_id, f => f.IndexFaker + 1)
             .RuleFor(x => x.Book_id, f => f.Random.Number(1, count - 1))
             .RuleFor(x => x.User_id, f => f.Random.Number(1, count - 1));
        }

        public static Faker<BookFolder> GetBookFolderRule(int count)
        {
            return new Faker<BookFolder>()
             .RuleFor(x => x.Id, f => f.IndexFaker + 1)
             .RuleFor(x => x.Book_id, f => f.Random.Number(1, count - 1))
             .RuleFor(x => x.Folder_id, f => f.Random.Number(1, count - 1));
        }
    }
}
