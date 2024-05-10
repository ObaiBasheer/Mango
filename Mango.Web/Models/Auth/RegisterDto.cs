using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public record RegisterDto
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; init; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; init; }

        public string? Name { get; init; }
        public string? PhoneNumber { get; init; }

        public string? Role { get; set; }
    }
}
