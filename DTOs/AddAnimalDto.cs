namespace Muuki.DTOs
{
    public class AddAnimalDto
    {
        public required string Type { get; set; }
        public required string Breed { get; set; }
        public int Count { get; set; } = 1;
    }
}