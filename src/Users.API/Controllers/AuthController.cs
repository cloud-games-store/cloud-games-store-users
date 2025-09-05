using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Application.DTOs;
using Users.Application.Interfaces;

namespace Users.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.Login(dto);

            if (!result.Success)
            {
                return Unauthorized(result.Error?.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
