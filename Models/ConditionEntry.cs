namespace Muuki.Models
{
    public class ConditionEntry
    {
        public DateTime Timestamp { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double Pollution { get; set; }
        public double FoodKg { get; set; }
        public double WaterLiters { get; set; }
        public int FoodFrequencyDays { get; set; }
        public int WaterFrequencyDays { get; set; }
    }
}
