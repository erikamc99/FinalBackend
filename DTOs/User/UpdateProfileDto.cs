using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class UpdateProfileDto
    {
        [Url]
        public string AvatarUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }
    }
}