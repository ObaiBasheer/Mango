namespace Mango.Services.AuthAPI.Models.Dtos
{
    public record LoginResponseDto
    {
        public UserDto? User { get; set; }
        public string? Token { get; set; }
    }
}
