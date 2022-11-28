using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Repository
{
    public interface IMemberRepository

    {
        Task<MemberDTO> AddMember(MemberDTO member);
        Task<IEnumerable<MemberDTO>> GetMemberDetails();
        Task<ResponseDTO> AssigningTask(AssigningTaskDTO assigningTaskDTO);
        Task<AssigningTaskDTO> GetAssigedTask(int MemberId);
        Task<ResponseDTO> UpdateAllocation(RequestUpdateAllocationDTO requestUpdateAllocationDTO);
       
    }
}
