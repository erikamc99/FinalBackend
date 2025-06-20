namespace Muuki.DTOs
{
    public class CreateSpaceDto
    {
        public required string Name { get; set; }
        public required string Type { get; set; } 
    }

    public class UpdateSpaceDto
    {
        public required string Name { get; set; }
    }

    public class AddAnimalDto
    {
        public required string Type { get; set; }
        public int Quantity { get; set; }
        public List<string> Breeds { get; set; } = new List<string>();
    }

    public class UpdateAnimalQuantityDto
    {
        public required string AnimalId { get; set; }
        public int Quantity { get; set; }
    }
}