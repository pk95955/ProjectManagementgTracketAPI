using ProjectManagementTracketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Repository
{
    public interface IUserRepository
    {
         Task<(bool, string)> VerifyUser(string userName, string password);
    }
}
