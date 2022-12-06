using ProjectManagementTracketAPI.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
namespace ProjectManagementTracketAPI.Token
{
    public class TokenGeneration
    {           
        public string TokenGenration(User user, string secretKey)
        { 
            // Create security token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            // Create private key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(secretKey);

            // Create Jwt Decsriptor
            string roleName = user.UserId == 1 ? "manager" : "member";
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Role,roleName)
                        }
                ),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature
                )
            };
            // Create Token 
            var token = tokenHandler.CreateToken(tokenDescriptor);

         return (tokenHandler.WriteToken(token));
        } 
    }
}
