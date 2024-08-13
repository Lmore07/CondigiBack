using CondigiBack.Libs.Responses;
using CondigiBack.Modules.Companies.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CondigiBack.Modules.Companies.DTOs.CompanyDTO;

namespace CondigiBack.Modules.Companies.Controllers
{
    [Route("api/companies")]
    [Produces("application/json")]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [ProducesResponseType<PaginatedResponse<List<AllCompanies>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get all companies")]
        [HttpGet(), AllowAnonymous]
        public async Task<IActionResult> GetCompanies([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _companyService.GetCompanies(currentPage, pageSize);
            return StatusCode(response.StatusCode, response);
        }
    }
}
