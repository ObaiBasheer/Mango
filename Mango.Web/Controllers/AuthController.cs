using Mango.Web.Models;
using Mango.Web.Services.Auth;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
                return View();
            }

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

        public IActionResult Logout()
        {
            return View();
        }
    }
}
