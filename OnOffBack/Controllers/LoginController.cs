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
        public async Task<ActionResult<ResponseGenericApi<LoginResponseDto>>> Login(LoginDto loginDto)
        {
            var user = await _adminInterfaces.usersRepository.GetByEmail(loginDto.NameUser);

            if (user == null)
            {
                return Unauthorized(new ResponseGenericApi<LoginResponseDto>("Usuario, contraseña incorrectos o cuenta inactiva", false));
            }

            var passwordHash = _adminInterfaces.utilsFunctionsRepository.DecodeMd5(loginDto.PasswordUser);

            if (user.PasswordUser != passwordHash)
            {
                return Unauthorized(new ResponseGenericApi<LoginResponseDto>("Usuario o contraseña incorrectos", false));
            }

            var token = _adminInterfaces.utilsFunctionsRepository.GenerateTokenJWT(user);
            LoginResponseDto loginResponseDto = new LoginResponseDto
            {
                Token = token,
                IdUser = user.id.ToString()
            };
            return Ok(new ResponseGenericApi<LoginResponseDto>(loginResponseDto, true));
        }
    }
}
