using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementTracketAPI.DbContexts;
using ProjectManagementTracketAPI.Models;
using ProjectManagementTracketAPI.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContexts _db;
        private readonly IMemoryCache _memoryCache;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly IEnumerable<string> _audience;


        public UserRepository(ApplicationDbContexts db, IMemoryCache memoryCache,
            string secretKey, string issuer, IEnumerable<string> audience)
        {
            _db = db;
            _secretKey = secretKey;
            _memoryCache = memoryCache;
            _issuer = issuer;
            _audience = audience;

        }
        public async Task<(bool,string)> VerifyUser(string userName, string password)
        {
          User user = await _db.User.Where(r => r.LoginName == userName && r.Password == password).FirstOrDefaultAsync();
           
            string newscretKey = _secretKey;
            string token = "";
            bool isVerifyUser = false;
            if (user != null)
            {
               Users d = new Users(user, _memoryCache);           
                isVerifyUser = true;
                token =  new TokenGeneration().TokenGenration(user, _secretKey, _issuer, _audience);
            }
              
            return  (isVerifyUser ,token);
        }
    }
}
