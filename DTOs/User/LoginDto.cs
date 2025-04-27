namespace Muuki.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public required string Password { get; set; }
    }
}