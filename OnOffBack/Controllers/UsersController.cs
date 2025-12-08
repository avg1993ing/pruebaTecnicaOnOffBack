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
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _usersService.GetAll();
            var usersDto = _mapper.Map<IEnumerable<UsersDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UsersDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UsersDto userDto)
        {
            var user = _mapper.Map<Users>(userDto);
            var newUser = await _usersService.Create(user);
            var newUserDto = _mapper.Map<UsersDto>(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUserDto.id }, newUserDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UsersDto userDto)
        {
            if (id != userDto.id)
            {
                return BadRequest();
            }
            var user = _mapper.Map<Users>(userDto);
            await _usersService.Update(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _usersService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
