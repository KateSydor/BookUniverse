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
    public class GetUserBookTests

    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public GetUserBookTests()
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
        public async Task GetUserBook_ReturnsUserBook()
        {
            // Arrange
            int userId = 1;
            int bookId = 2;
            UserBook userBook = new UserBook { UserId = userId, BookId = bookId };

            _userBookRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<UserBook, bool>>>()))
                                   .ReturnsAsync(userBook);

            // Act
            UserBook actualUserBook = await _bookManagementService.GetUserBook(userId, bookId);

            // Assert
            Assert.Equal(userBook, actualUserBook);
        }
    }
}
