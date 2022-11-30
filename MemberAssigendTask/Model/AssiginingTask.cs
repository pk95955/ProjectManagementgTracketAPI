using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAssigendTask.Model
{
    public class AssigningTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual int MemberId { get; set; }
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
