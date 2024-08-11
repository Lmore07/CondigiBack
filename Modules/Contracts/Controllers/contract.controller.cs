using System.Security.Claims;
using CondigiBack.Libs.Responses;
using CondigiBack.Modules.Contracts.DTOs;
using CondigiBack.Modules.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondigiBack.Modules.Contracts.Controllers;

[Route("api/contracts")]
[Produces("application/json")]
[ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
public class ContractController (ContractService service) : Controller 
{
    [HttpGet("list"), Authorize (Roles = "OWNER")]
    [ProducesResponseType<PaginatedResponse<List<ContractDto.ContractResponseDTO>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get contracts by user")]
    public async Task<IActionResult> GetContractsByUser([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new ErrorResponse<object> { Message = "User ID not found in token." });
        }

        var userId = Guid.Parse(userIdClaim.Value);
        Console.WriteLine(userId);
        var response = await service.GetContractsByUser(currentPage, pageSize, userId);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("{contractId}"), Authorize(Roles = "OWNER")]
    [ProducesResponseType<StandardResponse<ContractDto.ContractResponseDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get contract by ID")]
    public async Task<IActionResult> GetContractById(Guid contractId)
    {
        var response = await service.GetContractById(contractId);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("create"), Authorize(Roles = "OWNER")]
    [ProducesResponseType<StandardResponse<ContractDto.ContractResponseDTO>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Create a new contract")]
    public async Task<IActionResult> CreateContract([FromBody] ContractDto.CreateContractDTO contractDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new ErrorResponse<object> { Message = "User ID not found in token." });
        }

        var userId = Guid.Parse(userIdClaim.Value);

        var response = await service.CreateContract(contractDto, userId);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("{contractId}"), Authorize(Roles = "OWNER")]
    [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Update contract by ID")]
    public async Task<IActionResult> UpdateContract(Guid contractId, [FromBody] ContractDto.UpdateContractDTO contractDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new ErrorResponse<object> { Message = "User ID not found in token." });
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var response = await service.UpdateContract(contractId, contractDto, userId);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPatch("{contractId}/status"), Authorize(Roles="OWNER")]
    [ProducesResponseType<StandardResponse<ContractDto.ContractResponseDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Update contract status by ID")]
    public async Task<IActionResult> UpdateContractStatus(Guid contractId, [FromBody] ContractDto.UpdateStatusContractDTO statusDTO)
    {
        var response = await service.UpdateStatusContract(contractId, statusDTO.Status);
        return StatusCode(response.StatusCode, response);
    }
    
}