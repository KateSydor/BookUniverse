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
    public class UpdateUserBookTests
    {
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public UpdateUserBookTests()
        {
            _userBookRepositoryMock = new Mock<IUserBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _bookManagementService = new BookManagementService(
                new Mock<IBookRepository>().Object,
                _userBookRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task UpdateUserBook_ValidUserBook_CallsRepositoryUpdate()
        {
            // Arrange
            UserBook updatedUserBook = new UserBook
            {
                Id = 1,
                UserId = 3,
                BookId = 4
            };

            _userBookRepositoryMock.Setup(repo => repo.Update(It.IsAny<UserBook>()));

            // Act
            await _bookManagementService.UpdateUserBook(updatedUserBook);

            // Assert
            _userBookRepositoryMock.Verify(repo => repo.Update(It.IsAny<UserBook>()), Times.Once);
        }
    }
}
