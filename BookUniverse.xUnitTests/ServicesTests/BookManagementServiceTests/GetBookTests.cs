using AutoMapper;
using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.BookRepository;
using BookUniverse.DAL.Repositories.UserBookRepository;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.BookManagementServiceTests
{
    public class GetBookTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public GetBookTests()
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
        public async Task GetBook_ValidId_ReturnsBook()
        {
            // Arrange
            int bookId = 1;
            Book book = new Book { Id = bookId, Title = "Sample Book" };
            _bookRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Book, bool>>>())).ReturnsAsync(book);

            // Act
            Book result = await _bookManagementService.GetBook(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
        }
    }
}
