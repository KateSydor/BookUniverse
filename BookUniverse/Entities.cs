namespace BookUniverseConsole
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }

        public override string ToString()
        {
            return $"User: Id: {Id}\tUserName: {UserName}\tEmail: {Email}\tPassword: {Password}\tRole: {Role}\n";
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int NumberOfPages { get; set; }
        public double Rating { get; set; }
        public int CategoryId { get; set; }

        public override string ToString()
        {
            return $"User: Id: {Id}\tTitle: {Title}\tAuthor: {Author}\tDescription: {Description}\tNumberOfPages: {NumberOfPages}\tRating: {Rating}\tCategoryId: {CategoryId}\n";
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public override string ToString()
        {
            return $"Category: Id: {Id}\tCategoryName: {CategoryName}\n";
        }
    }

    public class Folder
    {
        public int Id { get; set; }
        public string FolderName { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"Folder: Id: {Id}\tFolderName: {FolderName}\tUserId: {UserId}\n";
        }
    }

    public class UserBook
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        public override string ToString()
        {
            return $"UserBook: Id: {Id}\tUserId: {UserId}\tBookId: {BookId}\n";
        }
    }

    public class BookFolder
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int FolderId { get; set; }

        public override string ToString()
        {
            return $"BookFolder: Id: {Id}\tBookId: {BookId}\tFolderId: {FolderId}\n";
        }
    }

    public class Favourites
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"Favourites: Id: {Id}\tBookId: {BookId}\tUserId: {UserId}\n";
        }
    }
}
