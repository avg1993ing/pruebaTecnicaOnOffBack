﻿﻿﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.Services;
using Core.DTOs;

namespace OnOffBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ValidarEmailController : ControllerBase
    {
        public readonly IValidarEmailService _validarEmailService;

        public ValidarEmailController(IValidarEmailService validarEmailService)
        {
            _validarEmailService = validarEmailService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseGenericApi<object>>> Validar([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("El token es requerido.");
            }

            bool isValid = await _validarEmailService.ValidateEmail(token);
            if (!isValid)
            {
                return BadRequest("Token inválido o expirado.");
            }
            return Ok(new ResponseGenericApi<object>(new { message = "Correo validado exitosamente." }, true)); ;
        }
    }
}
