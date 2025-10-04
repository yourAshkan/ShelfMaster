using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShelfMaster.Infrastructure.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShelfMaster.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager, IMapper mapper) : ControllerBase
    {
        #region GenerateJwtToken
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString())
            };


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT : Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: config["JWT : Issuer"],
                audience: config["JWT : Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
            #endregion
    }
}
