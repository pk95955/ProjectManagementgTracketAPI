using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Repository;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ProjectManagementTracketAPI.Auth
{
    public class Auth :IAuth
    {
        private readonly string _key;
       // private readonly IUserRepository _userRepo;
        public Auth(string key)
        {
           // _userRepo = new UserRepository();
            _key = key;
        }
        public (bool, string) Authentication(string userName, string password)
        { 
        bool isSuccess= true;
        User user = new User() { 
            UserName = "PraveshKumar"
        };
       // (isSuccess, user) = await _userRepo.VerifyUser(userName, password);
            if (!isSuccess)
            {
                return (false, "");
            }
            // Create security token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            // Create private key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            // Create Jwt Decsriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.UserName)
                        }
                ),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature
                )
            };

            // Create Token 
            var token = tokenHandler.CreateToken(tokenDescriptor);

         return (isSuccess, tokenHandler.WriteToken(token));
        } 
    }
}
