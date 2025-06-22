using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Models;
using Muuki.Services;
using Muuki.DTOs;
using Muuki.Data;
using Muuki.Exceptions;
using MongoDB.Driver;
using System.Security.Claims;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/condition-check")]
    public class ConditionCheckController : ControllerBase
    {
        private readonly MongoContext _context;
        private readonly ConditionEvaluatorService _evaluator;

        public ConditionCheckController(MongoContext context, ConditionEvaluatorService evaluator)
        {
            _context = context;
            _evaluator = evaluator;
        }

        private string GetUserId()
        {
            return User.FindFirstValue("id") ?? throw new UnauthorizedException("Usuario no autenticado");
        }

        [HttpPost("{spaceId}")]
        public async Task<IActionResult> CheckSpaceConditions(string spaceId, [FromBody] ConditionEntryDto currentEntryDto)
        {
            var userId = GetUserId();

            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null)
                return NotFound(new { success = false, message = "Espacio no encontrado", data = (object)null });

            var allAnimals = space.Animals;
            var idealSettings = new List<ConditionSettings>();

            foreach (var animal in allAnimals)
            {
                foreach (var breed in animal.Breeds)
                {
                    var setting = await _context.ConditionSettings
                        .Find(c => c.Type == animal.Species && c.Breed == breed.Breed)
                        .FirstOrDefaultAsync();

                    if (setting != null)
                    {
                        idealSettings.Add(setting);
                    }
                }
            }

            if (!idealSettings.Any())
                throw new NotFoundException("No hay condiciones ideales establecidas");

            var avgTempMin = idealSettings.Average(c => c.TemperatureMin);
            var avgTempMax = idealSettings.Average(c => c.TemperatureMax);
            var avgHumidityMin = idealSettings.Average(c => c.HumidityMin);
            var avgHumidityMax = idealSettings.Average(c => c.HumidityMax);
            var avgPollutionMax = idealSettings.Average(c => c.PollutionMax);

            var ideal = new ConditionSettings
            {
                TemperatureMin = avgTempMin,
                TemperatureMax = avgTempMax,
                HumidityMin = avgHumidityMin,
                HumidityMax = avgHumidityMax,
                PollutionMax = avgPollutionMax,
                Type = "Mixed",
                Breed = "Mixed"
            };

            var entry = new ConditionEntry
            {
                Timestamp = currentEntryDto.Timestamp,
                Humidity = currentEntryDto.Humidity,
                Temperature = currentEntryDto.Temperature,
                Pollution = currentEntryDto.Pollution,
                FoodKg = currentEntryDto.FoodKg,
                WaterLiters = currentEntryDto.WaterLiters,
                FoodFrequencyDays = currentEntryDto.FoodFrequencyDays,
                WaterFrequencyDays = currentEntryDto.WaterFrequencyDays
            };

            var (wellbeingScore, alerts) = _evaluator.Evaluate(entry, ideal);

            var conditionToSave = new SpaceConditionEntry
            {
                SpaceId = space.Id,
                Timestamp = entry.Timestamp,
                Humidity = entry.Humidity,
                Temperature = entry.Temperature,
                Pollution = entry.Pollution,
                FoodKg = entry.FoodKg,
                WaterLiters = entry.WaterLiters,
                FoodFrequencyDays = entry.FoodFrequencyDays,
                WaterFrequencyDays = entry.WaterFrequencyDays,
                WellbeingScore = wellbeingScore,
                Alerts = alerts
            };

            await _context.SpaceConditions.InsertOneAsync(conditionToSave);

            return Ok(new
            {
                success = true,
                message = alerts.Count == 0 ? "Condiciones ideales" : "Condiciones mejorables",
                data = new
                {
                    wellbeingScore,
                    alerts
                }
            });
        }
    }
}