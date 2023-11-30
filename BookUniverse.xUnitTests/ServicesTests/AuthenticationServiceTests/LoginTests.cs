using System.Linq.Expressions;
using AutoMapper;
using BookUniverse.BLL.DTOs.UserDTOs;
using BookUniverse.BLL.Interfaces;
using BookUniverse.BLL.Services;
using BookUniverse.BLL.Utils;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.UserRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.AuthenticationServiceTests
{
    public class LoginTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggingService> _loggingServiceMock;
        private readonly IAuthenticationService _authenticationService;
        const string NOT_VALID = "Not valid credentials.";

        public LoginTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggingServiceMock = new Mock<ILoggingService>();
            _mapperMock = new Mock<IMapper>();

            _authenticationService = new AuthenticationService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggingServiceMock.Object
            );
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsLoggedInUser()
        {
            // Arrange
            LoginDto validLoginDto = new LoginDto
            {
                Username = "ExistingUser",
                Password = "CorrectPassword"
            };

            User existingUser = new User
            {
                Id = 1,
                Username = "ExistingUser",
                Email = "example@gmail.com",
                Password = Hasher.ComputeHash("CorrectPassword")
            };

            SetupUserRepository(existingUser);

            // Act
            User loggedInUser = await _authenticationService.Login(validLoginDto);

            // Assert
            Assert.NotNull(loggedInUser);
            Assert.Equal(existingUser, loggedInUser);
            _loggingServiceMock.Verify(logger => logger.LogInformation($"User {loggedInUser.Username} successfully logged in"), Times.Once);
        }

        [Fact]
        public async Task Login_NonExistingUser_ThrowsException()
        {
            // Arrange
            LoginDto nonExistingUserLoginDto = new LoginDto
            {
                Username = "NonExistingUser",
                Password = "SomePassword1234"
            };
            SetupUserRepository();

            // Act & Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(() => _authenticationService.Login(nonExistingUserLoginDto));

            Assert.Equal(NOT_VALID, exception.Message);
            _loggingServiceMock.Verify(logger => logger.LogError(nonExistingUserLoginDto, $"There is no user with username: {nonExistingUserLoginDto.Username} in the database"), Times.Once);
        }

        [Fact]
        public async Task Login_IncorrectPassword_ThrowsException()
        {
            // Arrange
            LoginDto invalidPasswordLoginDto = new LoginDto
            {
                Username = "ExistingUser",
                Password = "IncorrectPassword"
            };

            User existingUser = new User
            {
                Id = 1,
                Username = "ExistingUser",
                Password = Hasher.ComputeHash("CorrectPassword")
            };

            SetupUserRepository(existingUser);

            // Act & Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(() => _authenticationService.Login(invalidPasswordLoginDto));
            Assert.Equal(NOT_VALID, exception.Message);
            _loggingServiceMock.Verify(logger => logger.LogError(invalidPasswordLoginDto, NOT_VALID), Times.Once);

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
