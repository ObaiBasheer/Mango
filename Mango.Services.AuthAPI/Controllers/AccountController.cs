using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/v1/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAuthService _authService;
        protected ResponseDto _responseDto;

        public AccountController(ResponseDto responseDto, IAuthService authService)
        {
            _responseDto = responseDto;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var errorsMessage = await _authService.Register(register); 
            if(!string.IsNullOrEmpty(errorsMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorsMessage;
                return BadRequest(errorsMessage);
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var loginResponse = await _authService.Login(model);

            if(loginResponse.User == null)
            {
                _responseDto.IsSuccess = false ;
                _responseDto.Message = "Username or password is incorrect";
                return BadRequest(_responseDto);
            }
            _responseDto.IsSuccess=true;
            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            var assignRoleResponse = await _authService.AssignRole(model.Email!, model.RoleName!);

            if (!assignRoleResponse)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is incorrect";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
    }
}
