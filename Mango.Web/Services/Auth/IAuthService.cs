﻿using Mango.Web.Models;

namespace Mango.Web.Services.Auth
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginDto loginDto);
        Task<ResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<ResponseDto> AssignRoleAsync(RegisterDto register);
    }
}
