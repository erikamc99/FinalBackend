using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class AnimalCreateDto
    {
        [Required]
        public string Species { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public List<string> Breeds { get; set; } = new();
    }

    public class AnimalUpdateDto
    {
        [StringLength(30)]
        public string? Species { get; set; }

        [Range(1, int.MaxValue)]
        public int? Quantity { get; set; }
    }

    public class BreedCreateDto
    {
        [Required]
        [StringLength(30)]
        public string BreedName { get; set; }
    }
}