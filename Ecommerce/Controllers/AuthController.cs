using Ecommerce.DTOs.Auth;
using Ecommerce.DTOs.Common;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);

                return Ok( new ApiResponse<AuthResponseDto>
                {
                    Success = true,
                    Message = "Usuario registrado exitosamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Error al registrar usuario",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

    }
}
