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
      
       ResponseDTO AssigningTask();
        Task<AssigningTaskDTO> GetAssigedTask(int MemberId);
       
    }
}
