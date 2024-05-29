using Mango.Web.Models;
using Mango.Web.Services.RequestProvider;
using Mango.Web.Utility;

namespace Mango.Web.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IRequestProvider _requestProvider;
        public AuthService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<ResponseDto> AssignRoleAsync(RegisterDto register)
        {
            try
            {
                var coupon = await _requestProvider.PostAsync<RegisterDto>(new RequestDto()
                {
                    MethodType = SD.MethodType.POST,
                    Data = register,
                    URL = SD.AuthAPIBase + "api/v1/auth/Account/assignRole"
                });

                return new ResponseDto { IsSuccess = true, Message = "Successfully" } ;
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<ResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
               var result = await _requestProvider.PostAsync<ResponseDto>(new RequestDto()
                {
                    MethodType = SD.MethodType.POST,
                    Data = loginDto,
                    URL = SD.AuthAPIBase + "api/v1/auth/Account/login"
                }, UseToken:false);

                return (ResponseDto)result.Result!;

                
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<ResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var coupon = await _requestProvider.PostAsync<RegisterDto>(new RequestDto()
                {
                    MethodType = SD.MethodType.POST,
                    Data = registerDto,
                    URL = SD.AuthAPIBase + "api/v1/auth/Account/register"
                }, UseToken: false);

                return new ResponseDto { IsSuccess = true, Message = "Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
