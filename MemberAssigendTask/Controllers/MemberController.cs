using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAssigendTask.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepo;
        public MemberController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
           
        }
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> ReadMessageFromRabbitQueue(int memberId)
        {             
            return Ok(await _memberRepo.GetAssigedTask(memberId));
            
        }
        [HttpGet("read-message")]
       
        public void GetAssinedTask()
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
