using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAssigendTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepo;
        public MemberController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
           
        }
        [HttpGet("get-task")]
        public  void GetAssinedTask()
        {
            try
            {
                _memberRepo.AssigningTask();
            }
            catch (Exception ex)
            {
                var exce = ex;
                throw ex;
            }
        }
        
    }
}
