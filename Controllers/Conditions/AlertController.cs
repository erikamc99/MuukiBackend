using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Data;
using MongoDB.Driver;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/alerts")]
    public class AlertController : ControllerBase
    {
        private readonly MongoContext _context;

        public AlertController(MongoContext context)
        {
            _context = context;
        }

        [HttpGet("{spaceId}")]
        public async Task<IActionResult> GetCurrentAlerts(string spaceId)
        {
            var latest = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId)
                .SortByDescending(c => c.Timestamp)
                .FirstOrDefaultAsync();

            var alerts = latest?.Alerts ?? new List<string>();

            return Ok(new
            {
                success = true,
                message = "Alertas actuales del espacio",
                data = alerts
            });
        }
    }
}