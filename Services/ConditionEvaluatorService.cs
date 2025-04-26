using Muuki.Models;

namespace Muuki.Services
{
    public class ConditionEvaluatorService
    {
        public bool IsConditionOk(ConditionEntry entry, ConditionSettings ideal)
        {
            if (entry.Temperature < ideal.TemperatureMin || entry.Temperature > ideal.TemperatureMax)
                return false;

            if (entry.Humidity < ideal.HumidityMin || entry.Humidity > ideal.HumidityMax)
                return false;

            if (entry.Pollution > ideal.PollutionMax)
                return false;

            return true;
        }
    }
}