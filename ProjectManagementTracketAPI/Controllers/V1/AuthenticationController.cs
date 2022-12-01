using Microsoft.AspNetCore.Mvc;
using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Repository;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Controllers.V1
{
    [Route("projectmanagement/api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
       // private readonly IAuth auth;
        private readonly IUserRepository _userRepo;
        public AuthenticationController(  IUserRepository userRep )
        {
           _userRepo = userRep;
        }
        [HttpPost("authentication/{userName}/{password}")]
        public async Task<IActionResult> VerifyUser(string userName, string password)
        {         
            string tokenKey;
            bool isUserVerify;
            (isUserVerify, tokenKey) = await _userRepo.VerifyUser(userName, password);
            if (isUserVerify)
            {
                return Ok(tokenKey);
            }
            else
            {
                return Unauthorized();
            }          
        }
    }
}
