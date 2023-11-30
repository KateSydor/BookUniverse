using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.CategoryRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.CategoryServiceTests
{
    public class GetAllCategoriesTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ILoggingService> _loggingServiceMock;
        private readonly ICategoryService _categoryService;

        public GetAllCategoriesTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _loggingServiceMock = new Mock<ILoggingService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _loggingServiceMock.Object);
        }

        [Fact]
        public void GetAllCategories_ReturnsAllCategories()
        {
            // Arrange
            List<Category> categories = new List<Category>
            {
                new Category { CategoryName = "Fantasy" },
                new Category { CategoryName = "Science Fiction" },
            };

            _categoryRepositoryMock.Setup(repo => repo.GetAll())
                                   .Returns(categories.AsQueryable());

            // Act
            List<Category> result = _categoryService.GetAllCategories();

            // Assert
            Assert.Equal(categories, result);
        }
    }
}