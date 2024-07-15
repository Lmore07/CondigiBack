using CondigiBack.Libs.Responses;
using CondigiBack.Modules.Geography.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Security.Permissions;
using static CondigiBack.Modules.Geography.DTOs.GeographyDTO;

namespace CondigiBack.Modules.Geography.Controllers
{
    [Route("api/geography")]
    [Produces("application/json")]
    [ProducesResponseType<ErrorResponse<object>>(StatusCodes.Status500InternalServerError)]
    [ApiController]
    public class GeographyController : ControllerBase
    {
        private readonly GeographyService _geographyService;

        public GeographyController(GeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        [ProducesResponseType<StandardResponse<List<ProvinceResponseDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse<List<ProvinceResponseDTO>>>(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get all provinces")]
        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var response = await _geographyService.GetProvinces();
            return StatusCode(response.StatusCode, response);
        }

        [ProducesResponseType<StandardResponse<List<CantonResponseDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse<List<CantonResponseDTO>>>(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get all cantons from a province")]
        [HttpGet("cantons/{provinceId}")]
        public async Task<IActionResult> GetCantons(int provinceId)
        {
            var response = await _geographyService.GetCantons(provinceId);
            return StatusCode(response.StatusCode, response);
        }

        [ProducesResponseType<StandardResponse<List<ParishResponseDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse<List<ParishResponseDTO>>>(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get all parishes from a canton")]
        [HttpGet("parishes/{cantonId}")]
        public async Task<IActionResult> GetParishes(int cantonId)
        {
            var response = await _geographyService.GetParishes(cantonId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
