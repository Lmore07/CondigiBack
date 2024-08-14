using System.Security.Claims;
using CondigiBack.Libs.Enums;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Modules.Contracts.DTOs;
using CondigiBack.Modules.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondigiBack.Modules.Contracts.Controllers;

[Route("api/contracts")]
[Produces("application/json")]
[Authorize(Roles = "OWNER")]
[ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
[ApiController]
public class ContractController (ContractService service) : ControllerBase
{
    [HttpGet("list")]
    [ProducesResponseType<PaginatedResponse<List<ContractDto.ContractResponseDTO>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get contracts by user")]
    public async Task<IActionResult> GetContractsByUser([FromQuery] StatusContractEnum? status,
        [FromQuery] int pageSize = 10, [FromQuery] int currentPage = 1)
    {
        var userId = User.GetUserId();
        var response = await service.GetContractsByUser(currentPage, pageSize, userId, status);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("{contractId}")]
    [ProducesResponseType<StandardResponse<ContractDto.ContractResponseDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get contract by ID")]
    public async Task<IActionResult> GetContractById(Guid contractId)
    {
        var response = await service.GetContractById(contractId);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("create")]
    [ProducesResponseType<StandardResponse<ContractDto.ContractResponseDTO>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Create a new contract")]
    public async Task<IActionResult> CreateContract([FromBody] ContractDto.CreateContractDTO contractDto)
    {
        var userId = User.GetUserId();
        var response = await service.CreateContract(contractDto, userId);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("{contractId}")]
    [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Update contract by ID")]
    public async Task<IActionResult> UpdateContract(Guid contractId, [FromBody] ContractDto.UpdateContractDTO contractDto)
    {
        var userId = User.GetUserId();
        var response = await service.UpdateContract(contractId, contractDto, userId);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPatch("{contractId}/status")]
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