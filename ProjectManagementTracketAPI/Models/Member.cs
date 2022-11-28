using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Models
{    
    public class Member
    {
        [Key] 
        public int Id { get; set; }       
        public int MemberId { get; set; }
        [Required]
        public string MemberName { get; set; }
        [Required]
        public short ExperienceInYear { get; set; }
        [Required]
        public string CurrentProfileDesc { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public short AllocationPercentage { get; set; }
    }
    public class SkillsMaster
    {
        [Key]
        public short SkillId { get; set; }
        [Required]
        public string SkillName { get; set; }


    }
    public class SkillsTransaction
    { 
        public int MemberId { get; set; }
        [Display(Name = "SkillsMaster")]
        public virtual short SkillId { get; set; }
        [ForeignKey("SkillId")]
        public virtual SkillsMaster SkillsMaster { get; set; }
       
    }
    public class AssigningTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual int MemberId { get; set; }
        //[ForeignKey("MemberId")]
       // public virtual Member Member { get; set; }
        [Required]
        public string MemberName { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string Deliverbles { get; set; }
        [Required]
        public DateTime TaskStartDate { get; set; }
        [Required]
        public DateTime TaskEndDate { get; set; }

    }

}
