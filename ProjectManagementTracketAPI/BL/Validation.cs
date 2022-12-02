using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTrackerAPI.BL
{
    public class Validation
    {
        public (bool, string) AddMemberValidation(RequestMemberDTO memberDTO)
        {
            bool isSuccess = true;
            string message = "";
            if (memberDTO.ExperienceInYear <= 4)
            {
                isSuccess = false;
                message = "Only if the experiences is greater than 4, the member can be part of this project ";
            }
            else if (memberDTO.SkillSet.Count() < 3)
            {
                isSuccess = false;
                message = "Member should possess at least 3 skills";
            }
            else if (memberDTO.StartDate >= memberDTO.EndDate)
            {
                isSuccess = false;
                message = "Project end Date should be greater than projecet start date";
            }
            return (isSuccess, message);
        }
        public static void ValidateProjectAndTaskEnddate(DateTime projectEndDate, DateTime taskEndDate)
        {
            if (taskEndDate > projectEndDate)
                throw new InvalidProjectAndTaskEndDate();

        }
        public static  ResponseDTO ValidateTaskDate(AssigningTaskDTO assigningTaskDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            if (assigningTaskDTO.TaskStartDate >= assigningTaskDTO.TaskEndDate)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = "Task end date must be greater that task start date";

            }
            else
            {
                responseDTO.IsSuccess = true;
            }
            return responseDTO;

        }
    }
    [Serializable]
    class InvalidProjectAndTaskEndDate : Exception
    {
        public InvalidProjectAndTaskEndDate()
            : base(String.Format("Invalid Project and task end date"))
        {

        }
    }
}
