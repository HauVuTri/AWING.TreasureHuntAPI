using AWING.TreasureHuntAPI.Interfaces;
using AWING.TreasureHuntAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AWING.TreasureHuntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultsController : ControllerBase
    {
        private readonly IResultsService _resultsService;

        public ResultsController(IResultsService resultsService)
        {
            _resultsService = resultsService;
        }

        // POST: api/results/calculate
        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateAndStoreResult([FromBody] MapIdDto mapObj)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            try
            {
                var result = await _resultsService.CalculateAndStoreResult(mapObj.MapId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/results/{mapId}
        [HttpGet("{mapId}")]
        public async Task<IActionResult> GetResultByMapId(int mapId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _resultsService.GetResultByMapId(mapId, userId);

            if (result == null)
            {
                return NotFound("Result not found for the specified map.");
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllResultByUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _resultsService.GetAllResultByUser(userId);

            if (result == null)
            {
                return NotFound("Result not found for the specified map.");
            }

            return Ok(result);
        }
    }

}
