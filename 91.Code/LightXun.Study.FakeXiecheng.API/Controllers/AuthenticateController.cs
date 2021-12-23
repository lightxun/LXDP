using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.Models;
using LightXun.Study.FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthenticateController: ControllerBase
    {
        private readonly IConfiguration _configration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITouristRouteRepository _touristRouteRepository;

        public AuthenticateController(
            IConfiguration configuration, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITouristRouteRepository touristRouteRepository)
        {
            _configration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _touristRouteRepository = touristRouteRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody]LoginDto loginDto)
        {
            //1. 验证用户名密码
            var loginResult = await _signInManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                false,
                false);
            if (!loginResult.Succeeded)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(loginDto.Email);

            //2. 创建 JWT
            // HEADER
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            // PAYLOAD
            var claims = new List<Claim>
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var roleNames = await _userManager.GetRolesAsync(user);
            foreach(var roleName in roleNames)
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }

            // VERIFY SIGNITURE
            var secretByte = Encoding.UTF8.GetBytes(_configration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: _configration["Authentication:Issuer"],
                audience: _configration["Authentication:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            //3. return 200 OK + JWT
            return Ok(tokenStr);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            //1. 使用用户名创建用户对象
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            //2. hash密码, 保存用户
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            //3. 初始化购物车
            var shoppingCart = new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id
            };
            await _touristRouteRepository.CreateShoppingCartAsync(shoppingCart);
            await _touristRouteRepository.SaveAsync();

            //4. return
            return Ok();
        }
    }
}
