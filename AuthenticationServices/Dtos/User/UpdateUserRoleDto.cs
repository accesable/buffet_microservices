using System.ComponentModel.DataAnnotations;

namespace AuthenticationServices.Dtos.User
{
    public class UpdateUserRolesDto
    {
        [Required]
        public string userId { get; set; } = null!;
        [Required]
        public string[]? roleNames { get; set; } = null!;
    }
}
