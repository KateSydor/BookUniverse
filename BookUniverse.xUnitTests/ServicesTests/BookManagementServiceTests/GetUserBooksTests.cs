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
    public class GetUserBooksTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public GetUserBooksTests()
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
        public void GetUserBooks_ReturnsUserBooks()
        {
            // Arrange
            string userEmail = "user@example.com";
            List<UserBook> userBooks = new List<UserBook>
            {
                new UserBook { User = new User { Email = userEmail }, Book = new Book() },
                new UserBook { User = new User { Email = userEmail }, Book = new Book() },
            };

            _userBookRepositoryMock.Setup(repo => repo.GetAllByUser(It.IsAny<Expression<Func<UserBook, bool>>>()))
                           .Returns(userBooks.Select(ub => ub.Book).AsQueryable());

            // Act
            List<Book> actualUserBooks = _bookManagementService.GetUserBooks(userEmail);

            // Assert
            Assert.Equal(userBooks.Select(ub => ub.Book).ToList(), actualUserBooks);
        }
    }
}
