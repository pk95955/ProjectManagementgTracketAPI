using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Models
{
    public class AssigningTaskDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string TaskName { get; set; }
        public string Deliverbles { get; set; }    
        public DateTime TaskStartDate { get; set; }
        public DateTime TaskEndDate { get; set; }
    }
    
}
