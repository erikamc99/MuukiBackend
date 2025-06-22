using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Models;
using Muuki.Services;
using Muuki.Data;
using Muuki.Exceptions;
using MongoDB.Driver;

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

        [HttpPost("{spaceId}")]
        public async Task<IActionResult> CheckSpaceConditions(string spaceId, [FromBody] ConditionEntry currentEntry)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId).FirstOrDefaultAsync();
            if (space == null) return NotFound(new { success = false, message = "Espacio no encontrado", data = (object)null });

            var allAnimals = space.Animals;
            var idealSettings = new List<ConditionSettings>();
            var evaluatedAnimals = new List<object>();

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
                        evaluatedAnimals.Add(new { animal.Species, Breed = breed.Breed });
                    }
                }
            }

            if (!idealSettings.Any())
                throw new NotFoundException("No hay condiciones ideales en este espacio");

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

            var isOk = _evaluator.IsConditionOk(currentEntry, ideal);

            var conditionToSave = new SpaceConditionEntry
            {
                SpaceId = space.Id,
                Timestamp = currentEntry.Timestamp,
                Humidity = currentEntry.Humidity,
                Temperature = currentEntry.Temperature,
                Pollution = currentEntry.Pollution
            };

            await _context.SpaceConditions.InsertOneAsync(conditionToSave);

            return Ok(new
            {
                success = true,
                message = isOk ? "Condiciones ideales" : "Condiciones mejorables",
                data = new
                {
                    isOk,
                    ideal,
                    evaluatedAnimals
                }
            });
        }
    }
}