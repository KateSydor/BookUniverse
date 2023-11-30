using BookUniverse.BLL.DTOs.UserDTOs;
using BookUniverse.BLL.Interfaces;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.UserRepository;
using Moq;
using Xunit;

namespace BookUniverse.xUnitTests.ServicesTests.AuthenticationServiceTests
{
    public class RegisterTests
    {
        private readonly Mock<IAuthenticationService> _authenticationService;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public RegisterTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationService = new Mock<IAuthenticationService>();
        }

        [Fact]
        public async Task Register_ValidUser_CallsRepositoryCreate()
        {
            // Arrange
            RegistrationDto registrationDto = CreateDTO();

            _userRepositoryMock.Setup(repo => repo.Create(It.IsAny<User>()));
            _authenticationService.Setup(auth => auth.Register(It.IsAny<RegistrationDto>()))
                .Returns((RegistrationDto dto) => Task.Run(() => _userRepositoryMock.Object.Create(new User())));

            // Act
            await _authenticationService.Object.Register(registrationDto);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Register_PasswordsDoNotMatch_ThrowsArgumentException()
        {
            // Arrange
            RegistrationDto registrationDto = CreateDTO("CreateDTO");

            string expectedMessage = "Passwords don't match";
            _authenticationService.Setup(auth => auth.Register(It.IsAny<RegistrationDto>()))
                .ThrowsAsync(new ArgumentException(expectedMessage));

            // Act & Assert
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _authenticationService.Object.Register(registrationDto));
            Assert.Equal(expectedMessage, exception.Message);
        }

        private RegistrationDto CreateDTO(string pass = "Test1234")
        {
            return new RegistrationDto
            {
                Username = "ExampleUsername",
                Email = "EmailExample@gmail.com",
                Password = "Test1234",
                RepeatPassword = pass
            };
        }
    }
}
