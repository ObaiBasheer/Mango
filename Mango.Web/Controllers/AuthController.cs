using Mango.Web.Models;
using Mango.Web.Services;
using Mango.Web.Services.Auth;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (dto == null)
            {
                return BadRequest("DTO is null.");
            }

            ResponseDto response = await _authService.LoginAsync(dto);

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = "Username or password is incorrect";
                ModelState.AddModelError("CustomError", response?.Message!);
                return View(dto);
            }

            LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result)!)!;

          await  SignInUser(loginResponse);
            _tokenProvider.SetToken(loginResponse.Token!);
            TempData["success"] = "Welcome";



            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (dto == null)
            {
                return BadRequest("DTO is null.");
            }

            ResponseDto response = await _authService.RegisterAsync(dto);

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response!.Message;
                return View();
            }

            if (string.IsNullOrEmpty(dto.Role))
            {
                dto.Role = SD.RoleCustomer;
            }

            ResponseDto assignRole = await _authService.AssignRoleAsync(dto);

            if (assignRole == null || !assignRole.IsSuccess)
            {
                TempData["error"] = assignRole!.Message;
                return View();
            }

            TempData["success"] = "Registration Successful";
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();

            return RedirectToAction("Index", "Home");
        }


        private async Task SignInUser(LoginResponseDto responseDto)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(responseDto.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Name)!.Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(t => t.Type == "role")!.Value));

            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principle);
        }
    }
}
