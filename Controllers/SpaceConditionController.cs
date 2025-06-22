using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Data;
using Muuki.DTOs;
using MongoDB.Driver;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/space-conditions")]
    public class SpaceConditionController : ControllerBase
    {
        private readonly MongoContext _context;

        public SpaceConditionController(MongoContext context)
        {
            _context = context;
        }

        [HttpGet("{spaceId}/history")]
        public async Task<IActionResult> GetHistory(string spaceId, [FromQuery] int days = 7)
        {
            if (days <= 0) days = 7;
            var fromDate = DateTime.UtcNow.AddDays(-days);

            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .SortByDescending(c => c.Timestamp)
                .ToListAsync();

            var response = conditions.Select(c => new SpaceConditionEntryDto
            {
                Timestamp = c.Timestamp,
                Humidity = c.Humidity,
                Temperature = c.Temperature,
                Pollution = c.Pollution,
                FoodKg = c.FoodKg,
                WaterLiters = c.WaterLiters,
                FoodFrequencyDays = c.FoodFrequencyDays,
                WaterFrequencyDays = c.WaterFrequencyDays,
                WellbeingScore = c.WellbeingScore,
                Alerts = c.Alerts
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Histórico de condiciones del espacio",
                data = response
            });
        }
    }
}