namespace BookUniverse.BLL.Interfaces
{
    using BookUniverse.BLL.DTOs.BookDTOs;
    using BookUniverse.DAL.Entities;

    public interface IBookService
    {
        void AddBook(AddBookDto newBook);

        List<Book> GetAllBooks();

        List<Book> GetUserBooks(string userEmail);
    }
}
