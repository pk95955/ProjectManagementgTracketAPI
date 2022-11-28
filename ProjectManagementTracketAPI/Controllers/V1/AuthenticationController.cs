using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementTracketAPI.Auth;

namespace ProjectManagementTracketAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuth auth;
        public AuthenticationController(IAuth auth)
        {
            this.auth = auth;
        }
        [HttpPost("authentication")]
        public IActionResult VerifyUser(string userName, string password)
        {
            bool isSuccess;
            string tokenKey;

         (isSuccess, tokenKey) = auth.Authentication(userName, password);
            if (isSuccess)
            {
                return Ok(tokenKey);
            }
            else
            {
                return Unauthorized();
            }
           // _userRepo.VerifyUser(userName, password);
        }
    }
}
