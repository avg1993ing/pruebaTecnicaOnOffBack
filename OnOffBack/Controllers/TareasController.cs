using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnOffBack.Controllers
{
    //[Authorize]
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
        [Route("GetAllTasksByIdUser")]
        public async Task<ActionResult<ResponseGenericApi<IEnumerable<TaskUserDto>>>> GetAllTasksByIdUser(int id)
        {
            var tasks = await _taskUserService.GetAllTasksByIdUser(id);
            var tasksDto = _mapper.Map<IEnumerable<TaskUserDto>>(tasks);
            return Ok(new ResponseGenericApi<IEnumerable<TaskUserDto>>(tasksDto, true));
        }

        [HttpGet]
        [Route("GetAllTasksByIdUseFilter")]
        public async Task<ActionResult<ResponseGenericApi<IEnumerable<TaskUserDto>>>> GetAllTasksByIdUseFilter(int id,bool? estado)
        {
            var tasks = await _taskUserService.GetAllTasksByIdUseFilter(id, estado);
            var tasksDto = _mapper.Map<IEnumerable<TaskUserDto>>(tasks);
            return Ok(new ResponseGenericApi<IEnumerable<TaskUserDto>>(tasksDto, true));
        }


        [HttpGet]
        public async Task<ActionResult<ResponseGenericApi<IEnumerable<TaskUserDto>>>> GetAllTasks()
        {
            var tasks = await _taskUserService.GetAll();
            var tasksDto = _mapper.Map<IEnumerable<TaskUserDto>>(tasks);
            return Ok(new ResponseGenericApi<IEnumerable<TaskUserDto>>(tasksDto, true));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseGenericApi<TaskUserDto>>> GetTaskById(int id)
        {
            var task = await _taskUserService.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            var taskDto = _mapper.Map<TaskUserDto>(task);
            return Ok(new ResponseGenericApi<TaskUserDto>(taskDto, true));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseGenericApi<TaskUserDto>>> CreateTask(TaskUserDto taskDto)
        {
            var task = _mapper.Map<TaskUser>(taskDto);
            var newTask = await _taskUserService.Create(task);
            var newTaskDto = _mapper.Map<TaskUserDto>(newTask);
            return Ok(new ResponseGenericApi<TaskUserDto>(newTaskDto, true));
        }

        [HttpPut]
        public async Task<ActionResult<ResponseGenericApi<TaskUserDto>>> UpdateTask([FromQuery]int id, TaskUserDto taskDto)
        {
            if (id != taskDto.id)
            {
                return BadRequest();
            }
            var task = _mapper.Map<TaskUser>(taskDto);
            var taskUpdate = await _taskUserService.Update(task);
            var newTaskDto = _mapper.Map<TaskUserDto>(taskUpdate);
            return Ok(new ResponseGenericApi<TaskUserDto>(newTaskDto, true, "Se actualizó la tarea"));
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseGenericApi<bool>>> DeleteTask([FromQuery] int id)
        {
            var result = await _taskUserService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new ResponseGenericApi<bool>(result, true, "Se elimino la tarea"));
        }
    }
}
