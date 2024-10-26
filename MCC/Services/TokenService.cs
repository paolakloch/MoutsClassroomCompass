
using MCC.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MCC.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            Claim[] claims = new Claim[] {

                new Claim("email", user.Email),
                new Claim("id", user.Id)
            };

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes("fjlsdijf983fnsjd"));

            var signingCredentials = new SigningCredentials
                (key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                expires: DateTime.Now.AddDays(30),
                claims: claims,
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}