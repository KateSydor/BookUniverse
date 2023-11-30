using BookUniverse.BLL.Interfaces;
using BookUniverse.DAL.Entities;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.AuthenticationServiceTests
{
    public class LogoutTests
    {
        private readonly Mock<IAuthenticationService> _authenticationService;
        private readonly Mock<ILoggingService> _loggerMock;

        public LogoutTests()
        {
            _authenticationService = new Mock<IAuthenticationService>();
            _loggerMock = new Mock<ILoggingService>();
        }

        [Fact]
        public async Task Logout_UserIsNotLoggedIn_DoesNotLogInformation()
        {
            // Arrange
            _authenticationService.Setup(auth => auth.CurrentAccount).Returns((User)null);

            // Act
            _authenticationService.Object.Logout();

            // Assert
            _loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
