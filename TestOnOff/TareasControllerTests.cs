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
    public class TareasControllerTests
    {
        private readonly Mock<ITaskUserService> _taskUserServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBaseService<TaskUser>> _baseServiceMock;
        private readonly Mock<IValidator<TaskUser>> _validatorMock;
        private readonly TareasController _controller;

        public TareasControllerTests()
        {
            _taskUserServiceMock = new Mock<ITaskUserService>();
            _mapperMock = new Mock<IMapper>();
            _baseServiceMock = new Mock<IBaseService<TaskUser>>();
            _validatorMock = new Mock<IValidator<TaskUser>>();
            
            _controller = new TareasController(
                _mapperMock.Object,
                _baseServiceMock.Object,
                _validatorMock.Object,
                _taskUserServiceMock.Object
            );
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOk()
        {
            // Arrange
            var tasks = new List<TaskUser> { new TaskUser { id = 1, NameTask = "Test Task" } };
            var taskDtos = new List<TaskUserDto> { new TaskUserDto { id = 1, NameTask = "Test Task" } };

            _taskUserServiceMock.Setup(s => s.GetAll()).ReturnsAsync(tasks);
            _mapperMock.Setup(m => m.Map<IEnumerable<TaskUserDto>>(tasks)).Returns(taskDtos);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<TaskUserDto>>(okResult.Value);
            Assert.Equal(taskDtos, returnValue);
        }

        [Fact]
        public async Task GetTaskById_Exists_ReturnsOk()
        {
            // Arrange
            var task = new TaskUser { id = 1, NameTask = "Test Task" };
            var taskDto = new TaskUserDto { id = 1, NameTask = "Test Task" };

            _taskUserServiceMock.Setup(s => s.GetById(1)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TaskUserDto>(task)).Returns(taskDto);

            // Act
            var result = await _controller.GetTaskById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskUserDto>(okResult.Value);
            Assert.Equal(taskDto, returnValue);
        }

        [Fact]
        public async Task GetTaskById_NotExists_ReturnsNotFound()
        {
            // Arrange
            _taskUserServiceMock.Setup(s => s.GetById(1)).ReturnsAsync((TaskUser)null);

            // Act
            var result = await _controller.GetTaskById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTask_ValidTask_ReturnsCreatedAtAction()
        {
            // Arrange
            var taskDto = new TaskUserDto { NameTask = "New Task" };
            var task = new TaskUser { NameTask = "New Task" };
            var createdTask = new TaskUser { id = 1, NameTask = "New Task" };
            var createdTaskDto = new TaskUserDto { id = 1, NameTask = "New Task" };

            _mapperMock.Setup(m => m.Map<TaskUser>(taskDto)).Returns(task);
            _taskUserServiceMock.Setup(s => s.Create(task)).ReturnsAsync(createdTask);
            _mapperMock.Setup(m => m.Map<TaskUserDto>(createdTask)).Returns(createdTaskDto);
            _validatorMock.Setup(v => v.ValidateAsync(task, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());


            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetTaskById", createdAtActionResult.ActionName);
            Assert.Equal(createdTaskDto.id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(createdTaskDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateTask_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = new TaskUserDto { id = 2, NameTask = "Updated Task" };

            // Act
            var result = await _controller.UpdateTask(1, taskDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteTask_Exists_ReturnsNoContent()
        {
            // Arrange
            _taskUserServiceMock.Setup(s => s.Delete(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTask_NotExists_ReturnsNotFound()
        {
            // Arrange
            _taskUserServiceMock.Setup(s => s.Delete(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
