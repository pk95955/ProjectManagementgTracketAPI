using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Models.DTO;
using ProjectManagementTracketAPI.Repository;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Controllers.V1
{
    [Route("projectmanagement/api/v1/member")]
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
        [Authorize(Roles = "member")]
        public async Task<ResponseDTO> GetAssignedTask(int memberId)
        {
            return await _memberRepo.GetAssigedTask(memberId);
        }
    }
}
