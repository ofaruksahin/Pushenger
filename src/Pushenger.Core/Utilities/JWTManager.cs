using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities.Result;
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

        public static string GetToken(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            token = token.Replace("Bearer", "").TrimStart(' ');
            return token;
        }

        public static int GetUserId(HttpContext context,IUnitOfWork unitOfWork)
        {
            int userId = 0;
            var token = GetToken(context);
            if (String.IsNullOrWhiteSpace(token))
                return userId;
            IDataResult<Core.Entities.User> existUser = unitOfWork.UserRepository.CheckToken(token);
            if (existUser.Data != null)
                userId = existUser.Data.Id;
            return userId;
        }

        public static Core.Entities.User GetUser(int userId,IUnitOfWork unitOfWork)
        {
            User user = default;
            user = unitOfWork.UserRepository.GetUser(userId).Data;
            return user;
        }
    }
}
