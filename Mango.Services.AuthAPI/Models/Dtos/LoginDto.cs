using System.ComponentModel.DataAnnotations;

namespace Mango.Services.AuthAPI.Models.Dtos
{
    public record LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
