using Microsoft.IdentityModel.Tokens;
using Pushenger.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Pushenger.Core.Utilities
{
    public static class JWTManager
    {
        public static string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("CompanyId",user.CompanyId.ToString())
            };
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PushengerRestAPI"));
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
