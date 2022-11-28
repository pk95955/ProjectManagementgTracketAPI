using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string LoginName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
