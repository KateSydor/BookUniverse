using BookUniverse.BLL.DTOs.UserDTOs;
using BookUniverse.BLL.Interfaces;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.UserRepository;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.AuthenticationServiceTests
{
    public class EditUserTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationService;
        const string NOT_VALID = "Not valid credentials.";

        public EditUserTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationService = new Mock<IAuthenticationService>();
        }

        [Fact]
        public async Task EditUser_NullNewUserDto_ThrowsException()
        {
            // Arrange
            int userId = 1;
            EditUserDto nullNewUserDto = null;

            SetupRepositoryForExistingUser(userId, new User());
            SetupAuthenticationServiceForException(NOT_VALID);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _authenticationService.Object.EditUser(userId, nullNewUserDto));
        }

        private void SetupRepositoryForExistingUser(int userId, User user)
        {
            _userRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>()))
                      .ReturnsAsync(user);

            _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>()))
                      .Callback<User>(updatedUser =>
                      {
                          // Update the existing user with the values from the new user
                          user.Username = updatedUser.Username;
                          user.Email = updatedUser.Email;
                          // Update any other properties as needed
                      })
                      .Returns(Task.CompletedTask);
        }

        private void SetupAuthenticationService(User user) =>
            _authenticationService.Setup(auth => auth.CurrentAccount)
                                  .Returns(user);

        private void SetupAuthenticationServiceForException(string message) =>
            _authenticationService.Setup(auth => auth.EditUser(It.IsAny<int>(), It.IsAny<EditUserDto>()))
                                  .ThrowsAsync(new Exception(message));
    }
}
