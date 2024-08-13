using AWING.TreasureHuntAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AWING.TreasureHuntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private TreasureHuntDbContext _db;

        public HealthController(TreasureHuntDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"Connect DB : {_db.Database.CanConnect()}") ;
        }
    }
}
