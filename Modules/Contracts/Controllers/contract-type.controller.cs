using System.Security.Claims;
using CondigiBack.Libs.Responses;
using CondigiBack.Modules.Contracts.DTOs;
using CondigiBack.Modules.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondigiBack.Modules.Contracts.Controllers;

[Route("api/contract-types")]
[Produces("application/json")]
[ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
public class ContractTypeController(ContractTypeService service) : Controller
{
    [HttpGet("list"), Authorize (Roles = "OWNER")]
    [ProducesResponseType<PaginatedResponse<List<ContractTypeDto.ContractTypeResponseDTO>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get contract types")]
    public async Task<IActionResult> GetContractTypes([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
    {
        var response = await service.GetContractTypes(currentPage, pageSize);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("create"), Authorize(Roles = "OWNER")]
    [ProducesResponseType<StandardResponse<ContractTypeDto.CreateContractTypeDTO>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Create a new contract type")]
    public async Task<IActionResult> CreateContractType([FromBody] ContractTypeDto.CreateContractTypeDTO contractTypeDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new ErrorResponse<object> { Message = "User ID not found in token." });
        }

        var userId = Guid.Parse(userIdClaim.Value);

        var response = await service.CreateContractType(contractTypeDto, userId);
        return StatusCode(response.StatusCode, response);
    }
}