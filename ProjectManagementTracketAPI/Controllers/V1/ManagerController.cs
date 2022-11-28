
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
    public class ManagerController : ControllerBase
    {
        protected ResponseDTO responseDTO;
        private IMemberRepository _memberRepo;
        public ManagerController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
            this.responseDTO = new ResponseDTO();
        }
        [HttpPost]
        [Route("add-member")]
        public async Task<ResponseDTO> AddMember([FromBody] MemberDTO memberDTO)
        {
            if (memberDTO.ExperienceInYear <= 4)
            {
                ResponseDTO response = new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "Only if the experiences is greater than 4, the member can be part of this project ",
                   
                };
                return response;
            }
            else if(memberDTO.SkillSet.Count() < 3)
            {
                ResponseDTO response = new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "Member should possess at least 3 skills",

                };
                return response;

            }
            else if(memberDTO.StartDate > memberDTO.EndDate)
            {
                ResponseDTO response = new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "End Date should be greater than projecet start date",

                };
                return response;
            }
            else
            {
                MemberDTO _memberDTO = await _memberRepo.AddMember(memberDTO);
                ResponseDTO response = new ResponseDTO
                {
                    IsSuccess = true,
                    Message = "Data saved successfully",
                    Result = _memberDTO
                };
                return response;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("list")]
        public async Task<IEnumerable<MemberDTO>> GetMemberDetails()
        {
           return await _memberRepo.GetMemberDetails();

        }
        [HttpPost]
        [Route("assign-task")]
        public async Task<ResponseDTO> AssignTask([FromBody] AssigningTaskDTO assigningTaskDTO)
        {
            return await _memberRepo.AssigningTask(assigningTaskDTO);

        }
        [HttpPut]
        [Route("Update")]
        public async Task<ResponseDTO> UpdateAllocation(RequestUpdateAllocationDTO requestUpdateAllocationDTO)
        {
            return await _memberRepo.UpdateAllocation(requestUpdateAllocationDTO);
        }

    }
}
