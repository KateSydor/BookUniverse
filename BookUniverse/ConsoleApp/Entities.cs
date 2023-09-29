using System.Globalization;

namespace BookUniverseConsole
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }

        public override string ToString()
        {
            return $"User: Id: {Id}\tUserName: {Username}\tEmail: {Email}\tPassword: {Password}\tRole: {Role}\n";
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int Number_of_pages { get; set; }
        public double Rating { get; set; }
        public int Category_id { get; set; }

        public override string ToString()
        {
            return $"User: Id: {Id}\tTitle: {Title}\tAuthor: {Author}\tPath: {Path}\tDescription: {Description}\tNumberOfPages: {Number_of_pages}\tRating: {Rating}\tCategoryId: {Category_id}\n";
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Category_name { get; set; }

        public override string ToString()
        {
            return $"Category: Id: {Id}\tCategoryName: {Category_name}\n";
        }
    }

    public class Folder
    {
        public int Id { get; set; }
        public string Folder_name { get; set; }
        public int User_id { get; set; }

        public override string ToString()
        {
            return $"Folder: Id: {Id}\tFolderName: {Folder_name}\tUserId: {User_id}\n";
        }
    }

    public class UserBook
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Book_id { get; set; }
        public bool Is_favourite { get; set; }

        public override string ToString()
        {
            return $"UserBook: Id: {Id}\tUserId: {User_id}\tBookId: {Book_id}\tIsFavourite: {Is_favourite}\n";
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
}
