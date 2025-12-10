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
    public class UsersController : BaseController<Users, UsersDto>
    {
        public readonly IUsersService _usersService;
        public UsersController(IMapper mapper, IBaseService<Users> service, IValidator<Users> validator, IUsersService usersService) : base(mapper, service, validator)
        {
            _usersService = usersService;   
        }

        [HttpGet]
        public async Task<ActionResult<ResponseGenericApi<IEnumerable<UsersDto>>>> GetAllUsers()
        {
            var users = await _usersService.GetAll();
            var usersDto = _mapper.Map<IEnumerable<UsersDto>>(users);
            return Ok(new ResponseGenericApi<IEnumerable<UsersDto>>(usersDto, true));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseGenericApi<UsersDto>>> GetUserById(int id)
        {
            var user = await _usersService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UsersDto>(user);
            return Ok(new ResponseGenericApi<UsersDto>(userDto, true));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ResponseGenericApi<UsersDto>>> CreateUser(UsersDto userDto)
        {
            var user = _mapper.Map<Users>(userDto);
            user.PasswordUser = userDto.PasswordUser;
            var newUser = await _usersService.Create(user);
            var newUserDto = _mapper.Map<UsersDto>(newUser);
            return Ok(new ResponseGenericApi<UsersDto>(newUserDto, true , "Usuario creado correctamente \r\n por favor revisar su correo para activación de cuenta"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseGenericApi<UsersDto>>> UpdateUser(int id, UsersDto userDto)
        {
            if (id != userDto.id)
            {
                return BadRequest();
            }
            var user = _mapper.Map<Users>(userDto);
            var userUpdate = await _usersService.Update(user);
            var newUserDto = _mapper.Map<TaskUserDto>(userUpdate);
            return Ok(new ResponseGenericApi<TaskUserDto>(newUserDto, true));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseGenericApi<bool>>> DeleteUser(int id)
        {
            var result = await _usersService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new ResponseGenericApi<bool>(result, true));
        }
    }
}
