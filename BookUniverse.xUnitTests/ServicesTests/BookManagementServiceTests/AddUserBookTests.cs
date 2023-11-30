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
    public class AddUserBookTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public AddUserBookTests()
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
        public async Task AddUserBook_ValidUserBook_CallsRepositoryCreate()
        {
            // Arrange
            UserBook newUserBook = new UserBook
            {
                Id = 1,
                UserId = 3,
                BookId = 4
            };

            _userBookRepositoryMock.Setup(repo => repo.Create(It.IsAny<UserBook>()));

            // Act
            await _bookManagementService.AddUserBook(newUserBook);

            // Assert
            _userBookRepositoryMock.Verify(repo => repo.Create(It.IsAny<UserBook>()), Times.Once);
        }
    }
}
