using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task SignIn(ApplicationUser user)
        {
             await _signInManager.SignInAsync(user, true);
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegisterDto register)
        {
            if (register == null) throw new ArgumentNullException(nameof(register));

            ApplicationUser user = new()
            {
                Email = register.Email,
                UserName = register.Email,
                PhoneNumber = register.PhoneNumber,
                Name = register.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, register.Password!);

                if (result.Succeeded)
                {
                    var userData = _dbContext.ApplicationUsers.FirstOrDefault(u=>u.UserName == register.Email);

                    UserDto userDto = new UserDto()
                    {
                        Email = userData!.Email,
                        Name = userData.Name,
                        PhoneNumber = userData.PhoneNumber,
                        Id = userData.Id,

                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault()!.Description;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "Error encountered";
        }

        public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u=>u.UserName!.ToLower() == dto.Email!.ToLower());

            if (user == null)
            {
                return new LoginResponseDto() { User = null , Token = ""};
            }

            bool isValid = await ValidateCredentials(user!, dto.Password!);

            if (!isValid)
            {
                return new LoginResponseDto() { User = null , Token = ""};
            }
            var role = await _userManager.GetRolesAsync(user);
            //If the user was found generate JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user, role);

            UserDto userDto = new UserDto
            {
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
            };

            LoginResponseDto result = new LoginResponseDto()
            {
                User = userDto,
                Token= token,
            };

            return result;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u=>u.Email == email);

            if (user != null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    //create new role

                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                //Assign role
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }
    }
}
