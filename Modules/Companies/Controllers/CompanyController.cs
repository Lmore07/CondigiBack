using CondigiBack.Libs.Responses;
using CondigiBack.Modules.Companies.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CondigiBack.Modules.Companies.DTOs.CompanyDTO;

namespace CondigiBack.Modules.Companies.Controllers
{
    [Route("api/companies")]
    [Authorize(Roles = "OWNER")]
    [Produces("application/json")]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status403Forbidden)]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [ProducesResponseType<StandardResponse<List<UsersByCompanyResponseDTO>>>(StatusCodes.Status200OK)]
        [EndpointSummary("Get all users by company")]
        [HttpGet("users/{companyId}")]
        public async Task<IActionResult> GetUsersByCompany(Guid companyId)
        {
            var response = await _companyService.GetUsersByCompany(companyId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
