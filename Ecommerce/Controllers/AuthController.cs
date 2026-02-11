using Ecommerce.DTOs.Auth;
using Ecommerce.DTOs.Common;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

                return Ok(new ApiResponse<AuthResponseDto>
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

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);

                return Ok(new ApiResponse<AuthResponseDto>
                {
                    Success = true,
                    Message = "Login exitoso",
                    Data = result
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Error al iniciar sesión",
                    Errors = new List<string> { ex.Message }
                });

            }
        }

        [HttpGet("validate-email/{email}")]
        public async Task<ActionResult<ApiResponse<bool>>> ValidateEmail(string email)
        {
            try
            {
                var result = await _authService.UserExistsAsync(email);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error al verificar email",
                    Errors = new List<string>{ ex.Message }
                });
            }
        } 

    }
}
