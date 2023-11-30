using AutoMapper;
using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.BookRepository;
using BookUniverse.DAL.Repositories.UserBookRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.BookManagementServiceTests
{
    public class GetAllBooksTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public GetAllBooksTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _userBookRepositoryMock = new Mock<IUserBookRepository>();
            _mapperMock = new Mock<IMapper>();

            _bookManagementService = new BookManagementService(
                _bookRepositoryMock.Object,
                _userBookRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public void GetAllBooks_ReturnsListOfBooks()
        {
            // Arrange
            List<Book> books = new List<Book> { new Book { Id = 1, Title = "Book1" }, new Book { Id = 2, Title = "Book2" } };
            _bookRepositoryMock.Setup(repo => repo.GetAll()).Returns(books);

            // Act
            List<Book> result = _bookManagementService.GetAllBooks();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
