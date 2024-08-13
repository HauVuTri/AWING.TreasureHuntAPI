using AWING.TreasureHuntAPI.Interfaces;
using AWING.TreasureHuntAPI.Models;
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
    public class TreasureMapsController : ControllerBase
    {
        private readonly ITreasureMapService _treasureMapService;

        public TreasureMapsController(ITreasureMapService treasureMapService)
        {
            _treasureMapService = treasureMapService;
        }

        // POST: api/treasuremaps
        [HttpPost]
        public async Task<TreasureMap> CreateTreasureMap([FromBody] CreateTreasureMapDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var treasureMap = await _treasureMapService.CreateTreasureMap(dto, userId);
            return treasureMap;
        }

        // GET: api/treasuremaps/{mapId}
        [HttpGet("{mapId}")]
        public async Task<IActionResult> GetTreasureMap(int mapId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var treasureMap = await _treasureMapService.GetTreasureMapById(mapId, userId);

            if (treasureMap == null)
            {
                return NotFound("Treasure map not found.");
            }

            return Ok(treasureMap);
        }

        // GET: api/treasuremaps
        [HttpGet]
        public async Task<IActionResult> GetAllTreasureMaps()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var treasureMaps = await _treasureMapService.GetAllTreasureMaps(userId);
            return Ok(treasureMaps);
        }

        // DELETE: api/treasuremaps/{mapId}
        [HttpDelete("{mapId}")]
        public async Task<IActionResult> DeleteTreasureMap(int mapId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _treasureMapService.DeleteTreasureMap(mapId, userId);

            if (!result)
            {
                return NotFound("Treasure map not found.");
            }

            return NoContent();
        }
        [HttpGet("{mapId}/cells")]
        public async Task<IActionResult> GetCellsByMapId(int mapId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var cells = await _treasureMapService.GetCellsByMapId(mapId, userId);

            return Ok(cells);
        }
    }
}
