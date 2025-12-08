using Core.DTOs;
using Core.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace OnOffBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAdminInterfaces _adminInterfaces;

        public LoginController(IAdminInterfaces adminInterfaces)
        {
            _adminInterfaces = adminInterfaces;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _adminInterfaces.usersRepository.GetByName(loginDto.NameUser);

            if (user == null)
            {
                return Unauthorized();
            }

            var passwordHash = _adminInterfaces.utilsFunctionsRepository.DecodeMd5(loginDto.PasswordUser);

            if (user.PasswordUser != passwordHash)
            {
                return Unauthorized();
            }

            var token = _adminInterfaces.utilsFunctionsRepository.GenerateTokenJWT(user);

            return Ok(new { Token = token });
        }
    }
}
