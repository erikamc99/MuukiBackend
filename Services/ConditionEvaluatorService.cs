using Muuki.Models;

namespace Muuki.Services
{
    public class ConditionEvaluatorService
    {
        private const int ThresholdDays = 2;
        private const int TotalParams = 5;

        public (double wellbeingScore, List<string> alerts) Evaluate(ConditionEntry entry, ConditionSettings ideal)
        {
            var alerts = new List<string>();
            int inRange = 0;

            if (entry.Temperature >= ideal.TemperatureMin && entry.Temperature <= ideal.TemperatureMax)
                inRange++;
            else if (entry.Temperature < ideal.TemperatureMin)
                alerts.Add("Temperatura por debajo del mínimo");
            else
                alerts.Add("Temperatura por encima del máximo");

            if (entry.Humidity >= ideal.HumidityMin && entry.Humidity <= ideal.HumidityMax)
                inRange++;
            else if (entry.Humidity < ideal.HumidityMin)
                alerts.Add("Humedad por debajo del mínimo");
            else
                alerts.Add("Humedad por encima del máximo");

            if (entry.Pollution <= ideal.PollutionMax)
                inRange++;
            else
                alerts.Add("Polución por encima del máximo");

            if (entry.FoodKg > 0 && entry.FoodFrequencyDays > ThresholdDays)
                inRange++;
            if (entry.FoodKg <= 0)
                alerts.Add("Sin comida disponible");
            if (entry.FoodFrequencyDays <= ThresholdDays)
                alerts.Add("Próximo a requerir rellenar comida");

            if (entry.WaterLiters > 0 && entry.WaterFrequencyDays > ThresholdDays)
                inRange++;
            if (entry.WaterLiters <= 0)
                alerts.Add("Sin agua disponible");
            if (entry.WaterFrequencyDays <= ThresholdDays)
                alerts.Add("Próximo a requerir rellenar agua");

            double wellbeingScore = (double)inRange / TotalParams * 100.0;
            return (wellbeingScore, alerts);
        }
    }
}