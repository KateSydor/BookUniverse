using System.Linq.Expressions;
using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.CategoryRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.CategoryServiceTests
{
    public class CategoryExistsTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ILoggingService> _loggingServiceMock;
        private readonly ICategoryService _categoryService;

        public CategoryExistsTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _loggingServiceMock = new Mock<ILoggingService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _loggingServiceMock.Object);
        }


        [Fact]
        public async Task CategoryExists_ExistingCategory_ReturnsCategory()
        {
            // Arrange
            string categoryName = "Fantasy";
            Category existingCategory = new Category { CategoryName = categoryName };

            SetupCategoryRepository(existingCategory);

            // Act
            Category result = await _categoryService.CategoryExists(categoryName);

            // Assert
            Assert.Equal(existingCategory, result);
        }

        [Fact]
        public async Task CategoryExists_NonExistingCategory_ThrowsException()
        {
            // Arrange
            string categoryName = "NonExistingCategory";
            SetupCategoryRepository();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _categoryService.CategoryExists(categoryName));
        }

        private void SetupCategoryRepository(Category? testCategory = null)
        {
            _categoryRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Expression<Func<Category, bool>> expression) =>
                {
                    Func<Category, bool> compiledExpression = expression.Compile();

                    if (testCategory != null && compiledExpression(testCategory))
                    {
                        return testCategory;
                    }

                    return null;
                });
        }
    }
}