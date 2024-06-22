using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthKalumManagement.DTOs;
using AuthKalumManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthKalumManagement.Controllers
{
    [ApiController]
    [Route("authkalum-management/v1/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IConfiguration Configuration;
        private readonly SignInManager<ApplicationUser> SignInManager;

        public AccountController(UserManager<ApplicationUser> _UserManager, IConfiguration _Configuration, SignInManager<ApplicationUser> _SignInManager)
        {
            this.UserManager = _UserManager;
            this.Configuration = _Configuration;
            this.SignInManager = _SignInManager;
        }

        //evento para log in
        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var login = await this.SignInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if(login.Succeeded)
            {
                var user = await this.UserManager.FindByNameAsync(loginDTO.UserName);
                return BuildToken(user);
            }
            else
            {
                ModelState.AddModelError(string.Empty,"El login es invalido");
                return BadRequest(ModelState);
            }


        }



        private UserTokenDTO BuildToken(ApplicationUser user) //Metodo para generar tokens 
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("api", "AuthKalumManament"),
                new Claim("username", user.UserName),
                new Claim("email", user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() )
            };
            var roles = this.UserManager.GetRolesAsync(user);

            claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Configurations:JWT:Key"]));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);
            return new UserTokenDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }   
    }
}