using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Models
{
    public class MemberDTO
    {      
       public int Id { get; set; }
        public int MemberId { get; set; }            
        public string MemberName { get; set; }
        public short ExperienceInYear { get; set; }     
        public List<SkillSetDTO> SkillSet { get; set; }      
        public string CurrentProfileDesc { get; set; }
        public DateTime StartDate { get; set; }      
        public DateTime EndDate { get; set; }       
        public short AllocationPercentage { get; set; }
    }
    public class SkillSetDTO
    {
        public short SkillId { get; set; }
        public string SkillName { get; set; }
    }

    public class RequestMemberDTO
    {
        
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public short ExperienceInYear { get; set; }
        public List<RequestSkillSetDTO> SkillSet { get; set; }
        public string CurrentProfileDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public short AllocationPercentage { get; set; }
    }
    public class RequestSkillSetDTO
    {
        public short SkillId { get; set; }
    }
    public class AssigningTaskDTO
    {
       // public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string TaskName { get; set; }
        public string Deliverbles { get; set; }    
        public DateTime TaskStartDate { get; set; }
        public DateTime TaskEndDate { get; set; }
    }
    public class RequestUpdateAllocationDTO
    {
      //  public int Id { get; set; }
        public int MemberId { get; set; }
        //public short AllocationPercentage { get; set; }
    }
}
