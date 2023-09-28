using System.Globalization;

namespace BookUniverseConsole
{
    public class User
    {
        public int User_id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }

        public override string ToString()
        {
            return $"User: Id: {User_id}\tUserName: {Username}\tEmail: {Email}\tPassword: {Password}\tRole: {Role}\n";
        }
    }

    public class Book
    {
        public int Book_id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int Number_of_pages { get; set; }
        public double Rating { get; set; }
        public int Category_id { get; set; }

        public override string ToString()
        {
            return $"User: Id: {Book_id}\tTitle: {Title}\tAuthor: {Author}\tPath: {Path}\tDescription: {Description}\tNumberOfPages: {Number_of_pages}\tRating: {Rating}\tCategoryId: {Category_id}\n";
        }
    }

    public class Category
    {
        public int Category_id { get; set; }
        public string Category_name { get; set; }

        public override string ToString()
        {
            return $"Category: Id: {Category_id}\tCategoryName: {Category_name}\n";
        }
    }

    public class Folder
    {
        public int Folder_id { get; set; }
        public string Folder_name { get; set; }
        public int User_id { get; set; }

        public override string ToString()
        {
            return $"Folder: Id: {Folder_id}\tFolderName: {Folder_name}\tUserId: {User_id}\n";
        }
    }

    public class UserBook
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Book_id { get; set; }

        public override string ToString()
        {
            return $"UserBook: Id: {Id}\tUserId: {User_id}\tBookId: {Book_id}\n";
        }
    }

    public class BookFolder
    {
        public int Id { get; set; }
        public int Book_id { get; set; }
        public int Folder_id { get; set; }

        public override string ToString()
        {
            return $"BookFolder: Id: {Id}\tBookId: {Book_id}\tFolderId: {Folder_id}\n";
        }
    }

    public class Favourites
    {
        public int Favorites_id { get; set; }
        public int Book_id { get; set; }
        public int User_id { get; set; }

        public override string ToString()
        {
            return $"Favourites: Id: {Favorites_id}\tBookId: {Book_id}\tUserId: {User_id}\n";
        }
    }
}
