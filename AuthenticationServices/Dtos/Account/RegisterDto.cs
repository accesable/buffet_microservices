using System.ComponentModel.DataAnnotations;

namespace AuthenticationServices.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Contact { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
