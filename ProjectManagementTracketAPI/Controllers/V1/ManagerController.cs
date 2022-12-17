
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementTrackerAPI.BL;
using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Models.DTO;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Controllers.V1
{
    [Route("/projectmanagement/api/v1/manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        protected ResponseDTO responseDTO;
        private readonly IMemberRepository _memberRepo;
        public ManagerController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
            this.responseDTO = new ResponseDTO();
        }
        [Authorize(Roles ="manager")]
        [HttpPost]       
        [Route("add-member")]
        public async Task<ResponseDTO> AddMember([FromBody] RequestMemberDTO requestMemberDTO)
        {
            bool isSuccess;
            string message;
            (isSuccess, message) = new Validation().AddMemberValidation(requestMemberDTO);
            ResponseDTO response = new ResponseDTO();
            if (isSuccess)
            {
                MemberDTO _memberDTO = await _memberRepo.AddMember(requestMemberDTO);
                response.Message = "Member added successfully";
                response.Result = _memberDTO;                
            }
            else
            {
                response.Message = message;
            }
            response.IsSuccess = isSuccess;
            return response;
        }
       // [Authorize(Roles ="manager")]
        [HttpGet]
        [Route("list")]
        public async Task<IEnumerable<MemberDTO>> GetMemberDetails()
        {
           return await _memberRepo.GetMemberDetails();

        }
        [Authorize(Roles ="manager")]
        [HttpPost]
        [Route("assign-task")]
        public async Task<ResponseDTO> AssignTask([FromBody] AssigningTaskDTO assigningTaskDTO)
        {
            
            ResponseDTO responseDTO = new ResponseDTO();
            responseDTO = Validation.ValidateTaskDate(assigningTaskDTO);
            if(!responseDTO.IsSuccess)
            {
                return responseDTO;  
            }
            return await _memberRepo.AssigningTask(assigningTaskDTO);

        }
        [Authorize(Roles = "manager")]
        [HttpPut]
        [Route("update")]
        public async Task<ResponseDTO> UpdateAllocation(RequestUpdateAllocationDTO requestUpdateAllocationDTO)
        {
            return await _memberRepo.UpdateAllocation(requestUpdateAllocationDTO);
        }

    }
}
