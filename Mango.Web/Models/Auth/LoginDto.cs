using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
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
