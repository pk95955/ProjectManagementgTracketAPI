using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAssigendTask.Controllers
{
    [Route("/projectmanagement/api/v1/members")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepo;
        //
        private readonly ILogger<MemberController> _logger;

        //public ElasticSearchController(ILogger<ElasticSearchController> logger)
        //{
        //    _logger = logger;
       // }       
        //
        public MemberController(IMemberRepository memberRepo, ILogger<MemberController> logger)
        {
            _logger = logger;
            _memberRepo = memberRepo;
            _memberRepo.AssigningTask();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("list")]
        public async Task<IActionResult> GetAssinedTask(int memberId)
        {             
            return Ok(await _memberRepo.GetAssigedTask(memberId));
            
        }
        // GET: api/values  
        [HttpGet("GetRandomvalue")]       
        public int GetRandomvalue()
        {
            var random = new Random();
            var randomValue = random.Next(0, 100);
            _logger.LogInformation($"Random Value is {randomValue}");
            return randomValue;
        }
        [HttpGet("read-message")]       
        public void ReadMessageFromRabbitQueue()
        {
            try
            {
                _memberRepo.AssigningTask();
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }

    }
}
