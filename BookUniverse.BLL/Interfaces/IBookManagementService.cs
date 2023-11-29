namespace BookUniverse.BLL.Interfaces
{
    using BookUniverse.BLL.DTOs.BookDTOs;
    using BookUniverse.DAL.Entities;

    public interface IBookManagementService
    {
        void AddBook(AddBookDto newBook, Category category);

        List<Book> GetAllBooks();

        Task<Book> GetBook(int id);

        List<Book> GetUserBooks(string userEmail);

        Task AddUserBook(UserBook newUserBook);

        Task<UserBook> GetUserBook(int userId, int bookId);

        Task UpdateUserBook(UserBook updated);
    }
}
