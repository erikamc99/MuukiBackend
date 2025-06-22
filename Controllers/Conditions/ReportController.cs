using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Data;
using MongoDB.Driver;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly MongoContext _context;

        public ReportController(MongoContext context)
        {
            _context = context;
        }

        [HttpGet("{spaceId}")]
        public async Task<IActionResult> GetReport(string spaceId, [FromQuery] string period = "week")
        {
            var now = DateTime.UtcNow;
            DateTime fromDate = period switch
            {
                "day" => now.Date,
                "week" => now.AddDays(-7),
                "fortnight" => now.AddDays(-15),
                "month" => now.AddMonths(-1),
                "6months" => now.AddMonths(-6),
                "year" => now.AddYears(-1),
                _ => now.AddDays(-7)
            };

            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .ToListAsync();

            if (!conditions.Any())
                return Ok(new
                {
                    success = true,
                    message = "No hay datos para el período seleccionado",
                    data = new
                    {
                        avgWellbeing = 0.0,
                        records = 0,
                        totalAlerts = 0,
                        topAlerts = new List<object>()
                    }
                });

            var avgWellbeing = conditions.Average(c => c.WellbeingScore);
            var records = conditions.Count;
            var allAlerts = conditions.SelectMany(c => c.Alerts).ToList();
            var totalAlerts = allAlerts.Count;

            var topAlerts = allAlerts
                .GroupBy(a => a)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new { alert = g.Key, count = g.Count() })
                .ToList();

            return Ok(new
            {
                success = true,
                message = "Informe de condiciones para el período",
                data = new
                {
                    avgWellbeing,
                    records,
                    totalAlerts,
                    topAlerts
                }
            });
        }
    }
}