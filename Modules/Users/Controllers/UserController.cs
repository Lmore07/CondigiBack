using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Modules.Users.DTOs;
using CondigiBack.Modules.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CondigiBack.Modules.Companies.DTOs.CompanyDTO;
using static CondigiBack.Modules.Users.DTOs.UserDTO;

namespace CondigiBack.Modules.Users.Controllers
{
    [Route("api/users")]
    [Produces("application/json")]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status401Unauthorized)]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse<object>>(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Create a company by user")]
        [HttpPost("create-company")]
        public async Task<IActionResult> AddCompanyByUser([FromBody] UserDTO.CompanyRegistrationDTO companyRegistrationDTO)
        {
            var userId = User.GetUserId();
            var response = await _userService.AddCompanyByUser(userId, companyRegistrationDTO);
            return StatusCode(response.StatusCode, response);
        }

        [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse<object>>(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Create a user in a company")]
        [HttpPost("create-user")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO.RegistrationUserToCompanyDTO userRegistrationDTO)
        {
            var response = await _userService.AddUser(userRegistrationDTO);
            return StatusCode(response.StatusCode, response);
        }

        [ProducesResponseType<StandardResponse<CompaniesByUserResponseDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get all companies by user")]
        [HttpGet("companies")]
        public async Task<IActionResult> GetCompaniesByUser()
        {
            var userId = User.GetUserId();
            var response = await _userService.GetCompaniesByUser(userId);
            return StatusCode(response.StatusCode, response);
        }

    }
}
