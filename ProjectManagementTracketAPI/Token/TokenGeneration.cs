using ProjectManagementTracketAPI.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagementTracketAPI.Token
{
    public class TokenGeneration
    {           
        public string TokenGenration(User user, string secretKey, string issuer, IEnumerable<string> audiences)
        {
            //   // Create security token handler
            //   var tokenHandler = new JwtSecurityTokenHandler();
            //   // Create private key to Encrypted
            //   var tokenKey = Encoding.ASCII.GetBytes(secretKey);

            //   // Create Jwt Decsriptor
            //   string roleName = user.UserId == 1 ? "manager" : "member";
            //   var tokenDescriptor = new SecurityTokenDescriptor()
            //   {
            //       Subject = new ClaimsIdentity(
            //               new Claim[]
            //               {
            //                   new Claim(ClaimTypes.Name, user.UserName),
            //                   new Claim(ClaimTypes.Role,roleName)
            //               }
            //       ),
            //       Expires = DateTime.UtcNow.AddHours(1),
            //       SigningCredentials = new SigningCredentials(
            //                   new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature
            //       )
            //   };
            //   // Create Token 
            //   var token = tokenHandler.CreateToken(tokenDescriptor);

            //return (tokenHandler.WriteToken(token));


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
            claims.AddRange(audiences.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.UtcNow.AddHours(1), signingCredentials: credential);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        } 
    }
}
