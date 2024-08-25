using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Modules.Contracts.DTOs;
using CondigiBack.Modules.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CondigiBack.Modules.Contracts.DTOs.ContractAIDTO;
using static CondigiBack.Modules.Contracts.DTOs.ContractAIDTO.CreateReceiverCompany;

namespace CondigiBack.Modules.Contracts.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status404NotFound)]
    [ApiController]
    public class ContractAIController : ControllerBase
    {
        private readonly ContractAIService service;

        public ContractAIController(ContractAIService service)
        {
            this.service = service;
        }

        [HttpGet("/api/contracts-ai/companies")]
        [ProducesResponseType<StandardResponse<GetCompaniesDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Get all companies for send contract")]
        public async Task<IActionResult> GetCompanies()
        {
            var userId = User.GetUserId();
            var response = await service.GetCompanies(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("/api/contracts-ai/persons")]
        [ProducesResponseType<StandardResponse<string>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Get all persons for send contract")]
        public async Task<IActionResult> GetPersons()
        {
            var userId = User.GetUserId();
            var response = await service.GetPersons(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("/api/contracts-ai/create")]
        [ProducesResponseType<StandardResponse<ContractAIResponseCompanyToCompany>>(StatusCodes.Status201Created)]
        [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Create a new contract with AI")]
        [EndpointDescription(@"Verificar en el esquema los campos obligatorios.
 - El SenderId sólo se envía cuando es una empresa que va a enviar el contrato.
 - El ReceiverId puede ser el Id de una empresa o una persona.
 - Si el ReceiverType es Company (0), el ReceiverId debe ser el Id de una empresa o el ReceiverCompany debe ser enviado.
 - Si el ReceiverType es Person (1), el ReceiverId debe ser el Id de una persona o el ReceiverPerson debe ser enviado.
 - Asimismo, si el SenderType es 0 es una empresa, si es 1 es una persona.")]
        public async Task<IActionResult> CreateContractAi([FromBody] ContractAIGeneralDto contractDto)
        {
            var userId = User.GetUserId();
            var response = await service.CreateContractAi(contractDto, userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}