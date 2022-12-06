using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
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
    public class Users
    {
        public  IMemoryCache _memoryCache; 
        public int UserId { get; set; }
        public int MemberId { get; set; }
       
        public string UserName { get; set; }
      
        public string LoginName { get; set; }
        public Users(User user, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            this.UserId = user.UserId;
            this.MemberId = user.MemberId;
            this.UserName = user.UserName;
            //  System.Runtime.Caching.Memorycache.Default[this.UserName] = this;
           // memoryCache.Set(this.UserId, user.UserId);
            _memoryCache.Set(this.UserName, this);
        }
       

        public static String Current
        {
            get
            {
                var usernameid = System.Web.HttpContext.Current.User.Claims;
                var userName = System.Web.HttpContext.Current.User.Claims.FirstOrDefault().Value;
                //var user =System.Web.HttpContext _memoryCache.Get<Users>(userName);
                //System.Web.HttpContext.Current
                return userName;
            }
        }
    }
    
}
