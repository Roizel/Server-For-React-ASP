using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.Cars.Data.Identity;

namespace Web.Cars.Services
{
    public interface IJwtTokenService /*Polimorfizm:))*/
    {
        string CreateToken(AppUser user);
        string DeleteToken();
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public JwtTokenService(IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public string CreateToken(AppUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result; /*Get roles of user*/
            List<Claim> claims = new List<Claim>()
            {
                new Claim("name", user.UserName)
                //,
                //new Claim("photo", user.Photo)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes/*From string in bytes*/(_configuration.GetValue<String>("JwtKey"))); /*Create Key(appsettings.json)*/
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256); /*encryption key*/

            var jwt = new JwtSecurityToken( /*Create JWT Token and configurate it*/
                signingCredentials: signinCredentials, /*Give JWT Token signinCredentials*/
                expires: DateTime.Now.AddDays(1), /*Time of living of this JWT*/
                claims: claims /*Give JWT Token claims*/
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt); /*Return our JWT Token in string*/
        }

        public string DeleteToken()
        {
            throw new NotImplementedException();
        }
    }
}
