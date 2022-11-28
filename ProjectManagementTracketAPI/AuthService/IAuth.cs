using ProjectManagementTracketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.Auth
{
    public interface IAuth
    {
      (bool, string) Authentication(string userName, string password);
    }
}
