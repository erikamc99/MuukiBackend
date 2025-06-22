using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Data;
using Muuki.DTOs;
using MongoDB.Driver;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/wellbeing")]
    public class WellbeingController : ControllerBase
    {
        private readonly MongoContext _context;

        public WellbeingController(MongoContext context)
        {
            _context = context;
        }

        [HttpGet("{spaceId}")]
        public async Task<IActionResult> GetCurrentWellbeing(string spaceId)
        {
            var latest = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId)
                .SortByDescending(c => c.Timestamp)
                .FirstOrDefaultAsync();

            if (latest == null)
                return NotFound(new { success = false, message = "No hay datos de condiciones para este espacio", data = (object)null });

            var dto = new SpaceConditionEntryDto
            {
                Timestamp = latest.Timestamp,
                Humidity = latest.Humidity,
                Temperature = latest.Temperature,
                Pollution = latest.Pollution,
                FoodKg = latest.FoodKg,
                WaterLiters = latest.WaterLiters,
                FoodFrequencyDays = latest.FoodFrequencyDays,
                WaterFrequencyDays = latest.WaterFrequencyDays,
                WellbeingScore = latest.WellbeingScore,
                Alerts = latest.Alerts
            };

            return Ok(new
            {
                success = true,
                message = "Nivel de bienestar actual",
                data = dto
            });
        }
    }
}