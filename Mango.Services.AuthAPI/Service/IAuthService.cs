using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dtos;

namespace Mango.Services.AuthAPI.Service
{
    public interface IAuthService
    {
        public Task SignIn(ApplicationUser user);

        public Task<LoginResponseDto> Login(LoginDto dto);
        Task<bool> ValidateCredentials(ApplicationUser user, string password);
        public void Logout();
        public Task<string> Register(RegisterDto register);

        public Task<bool> AssignRole(string email, string roleName);
    }
}
