using AutoMapper;
using BookUniverse.BLL.DTOs.BookDTOs;
using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.BookRepository;
using BookUniverse.DAL.Repositories.UserBookRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.BookManagementServiceTests
{
    public class AddBookTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserBookRepository> _userBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IBookManagementService _bookManagementService;

        public AddBookTests()
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
        public void AddBook_ValidData_CreatesBook()
        {
            // Arrange
            Category category = new Category { Id = 1, CategoryName = "Fiction" };

            AddBookDto newBookDto = new AddBookDto
            {
                Title = "Sample Book",
                Author = "John Doe",
                Description = "Description",
                NumberOfPages = 145,
                CategoryName = category.CategoryName,
                Path = "somepath"
            };

            _mapperMock.Setup(mapper => mapper.Map<Book>(newBookDto, It.IsAny<Action<IMappingOperationOptions>>()))
                       .Returns(new Book());

            // Act
            _bookManagementService.AddBook(newBookDto, category);

            // Assert
            _bookRepositoryMock.Verify(repo => repo.Create(It.IsAny<Book>()), Times.Once);
        }
    }
}
