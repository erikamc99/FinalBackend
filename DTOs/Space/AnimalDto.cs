namespace Muuki.DTOs
{
    public class AnimalCreateDto
    {
        public required string Type { get; set; }
        public int Quantity { get; set; }
        public List<string> Breeds { get; set; } = new();
    }

    public class AnimalUpdateDto
    {
        public string? Type { get; set; }
        public int? Quantity { get; set; }
    }

    public class BreedCreateDto
    {
        public required string BreedName { get; set; }
    }
}