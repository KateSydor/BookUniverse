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
    public class GetUserFavouriteBooksTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public GetUserFavouriteBooksTests()
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
        public void GetUserFavouriteBooks_ReturnsUserFavouriteBooks()
        {
            // Arrange
            string userEmail = "userexample@gmail.com";
            List<UserBook> userFavouriteBooks = new List<UserBook>
            {
                new UserBook { User = new User { Email = userEmail }, Book = new Book(), IsFavourite = true },
                new UserBook { User = new User { Email = userEmail }, Book = new Book(), IsFavourite = true },
            };

            _userBookRepositoryMock.Setup(repo => repo.GetAllByUser(It.IsAny<Expression<Func<UserBook, bool>>>()))
                           .Returns(userFavouriteBooks.Select(ub => ub.Book).AsQueryable());

            // Act
            List<Book> actualUserFavouriteBooks = _bookManagementService.GetUserFavouriteBooks(userEmail);

            // Assert
            Assert.Equal(userFavouriteBooks.Select(ub => ub.Book).ToList(), actualUserFavouriteBooks);
        }
    }
}
