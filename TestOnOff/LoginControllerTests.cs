using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnOffBack.Controllers;

namespace TestOnOff
{
    public class LoginControllerTests
    {
        private readonly Mock<IAdminInterfaces> _adminInterfacesMock;
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly Mock<IUtilsFunctionsRepository> _utilsFunctionsRepositoryMock;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _adminInterfacesMock = new Mock<IAdminInterfaces>();
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _utilsFunctionsRepositoryMock = new Mock<IUtilsFunctionsRepository>();

            _adminInterfacesMock.Setup(a => a.usersRepository).Returns(_usersRepositoryMock.Object);
            _adminInterfacesMock.Setup(a => a.utilsFunctionsRepository).Returns(_utilsFunctionsRepositoryMock.Object);

            _controller = new LoginController(_adminInterfacesMock.Object);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsOk()
        {
            // Arrange
            var loginDto = new LoginDto { NameUser = "test", PasswordUser = "password" };
            var user = new Users { NameUser = "test", PasswordUser = "hashedpassword" };
            var token = "jwt_token";

            _usersRepositoryMock.Setup(repo => repo.GetByName(loginDto.NameUser)).ReturnsAsync(user);
            _utilsFunctionsRepositoryMock.Setup(repo => repo.DecodeMd5(loginDto.PasswordUser)).Returns("hashedpassword");
            _utilsFunctionsRepositoryMock.Setup(repo => repo.GenerateTokenJWT(user)).Returns(token);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value;
            Assert.NotNull(returnValue);
            var tokenProperty = returnValue.GetType().GetProperty("Token");
            Assert.NotNull(tokenProperty);
            Assert.Equal(token, tokenProperty.GetValue(returnValue, null));
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { NameUser = "test", PasswordUser = "password" };

            _usersRepositoryMock.Setup(repo => repo.GetByName(loginDto.NameUser)).ReturnsAsync((Users)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { NameUser = "test", PasswordUser = "password" };
            var user = new Users { NameUser = "test", PasswordUser = "hashedpassword" };

            _usersRepositoryMock.Setup(repo => repo.GetByName(loginDto.NameUser)).ReturnsAsync(user);
            _utilsFunctionsRepositoryMock.Setup(repo => repo.DecodeMd5(loginDto.PasswordUser)).Returns("wronghashedpassword");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
