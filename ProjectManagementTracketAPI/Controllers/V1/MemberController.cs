using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Models.DTO;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        protected ResponseDTO responseDTO;
        private IMemberRepository _memberRepo;
        public MemberController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
            this.responseDTO = new ResponseDTO();
        }
        [HttpGet]
        [Route("list")]
        [Authorize]
        public async Task<AssigningTaskDTO> GetAssignedTask(int memberId)
        {
            return await _memberRepo.GetAssigedTask(memberId);
        }
    }
}
