using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnOffBack.Controllers;

namespace TestOnOff
{
    public class UsersControllerTests
    {
        private readonly Mock<IUsersService> _usersServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBaseService<Users>> _baseServiceMock;
        private readonly Mock<IValidator<Users>> _validatorMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _usersServiceMock = new Mock<IUsersService>();
            _mapperMock = new Mock<IMapper>();
            _baseServiceMock = new Mock<IBaseService<Users>>();
            _validatorMock = new Mock<IValidator<Users>>();

            _controller = new UsersController(
                _mapperMock.Object,
                _baseServiceMock.Object,
                _validatorMock.Object,
                _usersServiceMock.Object
            );
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOk()
        {
            // Arrange
            var users = new List<Users> { new Users { id = 1, NameUser = "Test User" } };
            var userDtos = new List<UsersDto> { new UsersDto { id = 1, NameUser = "Test User" } };

            _usersServiceMock.Setup(s => s.GetAll()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UsersDto>>(users)).Returns(userDtos);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<UsersDto>>(okResult.Value);
            Assert.Equal(userDtos, returnValue);
        }

        [Fact]
        public async Task GetUserById_Exists_ReturnsOk()
        {
            // Arrange
            var user = new Users { id = 1, NameUser = "Test User" };
            var userDto = new UsersDto { id = 1, NameUser = "Test User" };

            _usersServiceMock.Setup(s => s.GetById(1)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UsersDto>(user)).Returns(userDto);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UsersDto>(okResult.Value);
            Assert.Equal(userDto, returnValue);
        }

        [Fact]
        public async Task GetUserById_NotExists_ReturnsNotFound()
        {
            // Arrange
            _usersServiceMock.Setup(s => s.GetById(1)).ReturnsAsync((Users)null);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var userDto = new UsersDto { NameUser = "New User" };
            var user = new Users { NameUser = "New User" };
            var createdUser = new Users { id = 1, NameUser = "New User" };
            var createdUserDto = new UsersDto { id = 1, NameUser = "New User" };

            _mapperMock.Setup(m => m.Map<Users>(userDto)).Returns(user);
            _usersServiceMock.Setup(s => s.Create(user)).ReturnsAsync(createdUser);
            _mapperMock.Setup(m => m.Map<UsersDto>(createdUser)).Returns(createdUserDto);
            _validatorMock.Setup(v => v.ValidateAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetUserById", createdAtActionResult.ActionName);
            Assert.Equal(createdUserDto.id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(createdUserDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ValidUser_ReturnsNoContent()
        {
            // Arrange
            var userDto = new UsersDto { id = 1, NameUser = "Updated User" };
            var user = new Users { id = 1, NameUser = "Updated User" };

            _mapperMock.Setup(m => m.Map<Users>(userDto)).Returns(user);
            _usersServiceMock.Setup(s => s.Update(user)).ReturnsAsync(true);
            _validatorMock.Setup(v => v.ValidateAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.UpdateUser(1, userDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var userDto = new UsersDto { id = 2, NameUser = "Updated User" };

            // Act
            var result = await _controller.UpdateUser(1, userDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteUser_Exists_ReturnsNoContent()
        {
            // Arrange
            _usersServiceMock.Setup(s => s.Delete(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_NotExists_ReturnsNotFound()
        {
            // Arrange
            _usersServiceMock.Setup(s => s.Delete(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
