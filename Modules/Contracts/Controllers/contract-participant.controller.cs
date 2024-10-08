using System.Security.Claims;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Modules.Contracts.DTOs;
using CondigiBack.Modules.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace CondigiBack.Modules.Contracts.Controllers;

[Route("api/contract-participants")]
[Produces("application/json")]
[ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
public class ContractParticipantController(ContractParticipantService service) : Controller
{
    [HttpPost("add-company")]
    [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Add company to contract")]
    public async Task<IActionResult> AddUserToContract([FromBody] ContractParticipantDTO.AddCompanyToContractDTO payload){
        var response = await service.AddCompanyToContract(payload);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("add-user")]
    [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Add user to contract")]
    public async Task<IActionResult> AddUserToContract([FromBody] ContractParticipantDTO.AddUserToContractDTO payload){
        var response = await service.AddUserToContract(payload);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPatch("update-status/{contractParticipantId}")]
    [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Update status user in contract")]
    public async Task<IActionResult> UpdateStatusContract([FromRoute] Guid contractParticipantId)
    {
        var response = await service.UpdateStaus(contractParticipantId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("update-signed/{contractId}")]
    [ProducesResponseType<StandardResponse<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [EndpointSummary("Update signed user in contract")]
    public async Task<IActionResult> UpdateSignedContract([FromRoute] Guid contractId)
    {
        var userId = User.GetUserId();
        var response = await service.UpdateSigned(userId, contractId);
        return StatusCode(response.StatusCode, response);
    }
}