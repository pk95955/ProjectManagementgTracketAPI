using Microsoft.AspNetCore.Mvc;
using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Models.DTO;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Controllers.V1
{
    [Route("projectmanagement/unused/api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public AuthenticationController(  IUserRepository userRep )
        {
           _userRepo = userRep;
        }
        [HttpPost("verifyUser")]
        public async Task<IActionResult> VerifyUser(string userName, string password)
        {
            try
            {
                string tokenKey;
                bool isUserVerify;
                (isUserVerify, tokenKey) = await _userRepo.VerifyUser(userName, password);
                ResponseDTO response = new ResponseDTO()
                {
                    IsSuccess = isUserVerify,
                    Message = tokenKey.ToString()
                };
                if (isUserVerify)
                {
                    return Ok(response);
                }
                else
                {
                    return Unauthorized();
                }
            }catch(Exception ex)
            {
                return Ok(ex.Message);

            }
        }
    }
}
