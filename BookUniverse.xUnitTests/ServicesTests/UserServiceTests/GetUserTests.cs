using System.Linq.Expressions;
using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.UserRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests
{
    public class GetUserTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;

        public GetUserTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUser_ExistingUser_ReturnsUser()
        {
            // Arrange
            string userEmail = "user@example.com";
            User existingUser = new User { Email = userEmail };
            SetupUserRepository(existingUser);

            // Act
            User result = await _userService.GetUser(userEmail);

            // Assert
            Assert.Equal(existingUser, result);
        }

        [Fact]
        public async Task GetUser_NonExistingUser_ReturnsNull()
        {
            // Arrange
            string userEmail = "nonexistinguser@example.com";
            SetupUserRepository();

            // Act
            User result = await _userService.GetUser(userEmail);

            // Assert
            Assert.Null(result);
        }

        private void SetupUserRepository(User? testUser = null)
        {
            _userRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((Expression<Func<User, bool>> expression) =>
                {
                    Func<User, bool> compiledExpression = expression.Compile();

                    if (testUser != null && compiledExpression(testUser))
                    {
                        return testUser;
                    }

                    return null;
                });
        }
    }
}
