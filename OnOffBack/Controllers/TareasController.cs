using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnOffBack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : BaseController<TaskUser, TaskUserDto>
    {
        public readonly ITaskUserService _taskUserService;
        public TareasController(IMapper mapper, IBaseService<TaskUser> service, IValidator<TaskUser> validator, ITaskUserService taskUserService) : base(mapper, service, validator)
        {
            _taskUserService = taskUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskUserService.GetAll();
            var tasksDto = _mapper.Map<IEnumerable<TaskUserDto>>(tasks);
            return Ok(tasksDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskUserService.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            var taskDto = _mapper.Map<TaskUserDto>(task);
            return Ok(taskDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskUserDto taskDto)
        {
            var task = _mapper.Map<TaskUser>(taskDto);
            var newTask = await _taskUserService.Create(task);
            var newTaskDto = _mapper.Map<TaskUserDto>(newTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTaskDto.id }, newTaskDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUserDto taskDto)
        {
            if (id != taskDto.id)
            {
                return BadRequest();
            }
            var task = _mapper.Map<TaskUser>(taskDto);
            await _taskUserService.Update(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskUserService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
