﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShopping.Data;
using OnlineShopping.Dtos.Auth;
using OnlineShopping.Models;
using OnlineShopping.Utility;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            ApplicationUser user = db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == login.UserName.ToLower());
            bool isValid = await userManager.CheckPasswordAsync(user, login.Password);
            if (isValid==false)
            {
                response.Result = new LoginResponseDto();
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ErrorMessages.Add("Имя пользователя или пароль неверны");
                return BadRequest(response);
            }
            var roles = await userManager.GetRolesAsync(user);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Name", user.Name),
                    new Claim("id", user.Id.ToString()),
                    new Claim("UserName", user.UserName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponse = new()
            {
                Email = user.Email,
                Token = tokenHandler.WriteToken(token) 
            };
            if(loginResponse.Email == null)
            {
                response.StatusCode=HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Имя пользователя или пароль неверны");
                return BadRequest(response);
            }
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess=true;
            response.Result = loginResponse;
            return Ok(response);
        }



        [HttpPost("register")]
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
