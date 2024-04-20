namespace Mango.Web.Models
{ 
    public record LoginResponseDto
    {
        public UserDto? User { get; set; }
        public string? Token { get; set; }
    }
}
