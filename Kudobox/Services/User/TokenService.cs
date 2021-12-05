using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Kudobox.Helpers.Constants;
using Microsoft.IdentityModel.Tokens;

namespace Kudobox.Services.User
{
    public static class TokenService
    {
        public static string GenerateToken(Models.User.User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.Name),
                new(ClaimTypes.Surname, user.Surname),
                new(ClaimTypes.DateOfBirth, user.Birthday.ToShortDateString()),
                new("Avatar", user.Avatar)
            };
            
            claims.AddRange(user.Roles.Split(';').ToList().Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigurationConstants.TOKEN_SECRET_KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}