using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.CategoryRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.CategoryServiceTests
{
    public class AddCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ILoggingService> _loggingServiceMock;
        private readonly ICategoryService _categoryService;

        public AddCategoryTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _loggingServiceMock = new Mock<ILoggingService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _loggingServiceMock.Object);
        }

        [Fact]
        public async Task AddCategory_SuccessfullyAddsCategory()
        {
            // Arrange
            string categoryName = "NewCategory";

            // Act
            await _categoryService.AddCategory(categoryName);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.Create(It.IsAny<Category>()), Times.Once);
        }
    }
}
