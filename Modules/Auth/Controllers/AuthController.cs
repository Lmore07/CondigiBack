using CondigiBack.Libs.Middleware;
using CondigiBack.Libs.Responses;
using CondigiBack.Modules.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CondigiBack.Modules.Auth.DTOs.AuthDTO;
using static CondigiBack.Modules.Geography.DTOs.GeographyDTO;

namespace CondigiBack.Modules.Auth.Controllers
{
    [Route("api/auth")]
    [Produces("application/json")]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [ProducesResponseType<StandardResponse<Boolean>>(StatusCodes.Status201Created)]
        [ProducesResponseType<BadRequestResponse<object>>(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Register a new user")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRequest)
        {
            var response = await _authService.CreateUser(userRequest);
            return StatusCode(response.StatusCode, response);
        }

        [ProducesResponseType<StandardResponse<UserLoginResponseDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse<object>>(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Login a user")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userRequest)
        {
            var response = await _authService.Login(userRequest);
            return StatusCode(response.StatusCode, response);
        }
    }
}