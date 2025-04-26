namespace Muuki.DTOs
{
    public class AnimalCreateDto
    {
        public required string Type { get; set; }
        public required string Breed { get; set; }
        public int Quantity { get; set; }
    }

    public class AnimalUpdateDto
    {
        public required string Type { get; set; }
        public required string Breed { get; set; }
    }
    public class BreedCreateDto
    {
        public required string BreedName { get; set; }
    }
}