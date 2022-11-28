using Microsoft.EntityFrameworkCore;
using ProjectManagementTracketAPI.DbContexts;
using ProjectManagementTracketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContexts _db;
       
        public UserRepository(ApplicationDbContexts db)
        {
            _db = db;           
        }
        public async Task<(bool,User) > VerifyUser(string userName, string password)
        {
          User user = await _db.User.Where(r => r.LoginName == userName && r.Password == password).FirstOrDefaultAsync();
            bool isVerify = false;
            if (user != null)
                isVerify = true;
            return (isVerify, user);
        }
    }
}
