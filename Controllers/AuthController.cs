using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data;
using OnlineShopping.Dtos.Auth;
using OnlineShopping.Models;
using OnlineShopping.Utility;
using System.Net;

namespace OnlineShopping.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext db;
        private ApiResponse response;
        private string secretKey;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AuthController(AppDbContext db, IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            response = new ApiResponse();
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            ApplicationUser userFromDb = db.ApplicationUsers.FirstOrDefault(u=>u.UserName.ToLower()==requestDto.UserName.ToLower());
            if (userFromDb != null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Имя пользователя уже занято");
                return BadRequest(response);
            }
            ApplicationUser newUser = new()
            {
                UserName = requestDto.UserName,
                Email = requestDto.Email,
                NormalizedEmail = requestDto.UserName.ToUpper(),
                Name = requestDto.Name
            };
            try
            {
                var result = await userManager.CreateAsync(newUser, requestDto.Password);
                if (result.Succeeded)
                {
                    if (!roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }
                    if (requestDto.Role.ToLower() == SD.Role_Admin)
                    {
                        await userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    }
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                    return Ok(response);
                }
            }
            catch (Exception)
            {

            }
            response.StatusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages.Add("Ошибка при регистрации");
            return BadRequest(response);
        }
    }
}
