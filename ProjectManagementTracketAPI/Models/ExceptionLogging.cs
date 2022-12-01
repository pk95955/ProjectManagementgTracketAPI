using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Models
{
    public class ExceptionLog
    {
        [Key]
        public long Id { get; set; } 
        public DateTime  LogDate { get; set; }
        public string LogMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
