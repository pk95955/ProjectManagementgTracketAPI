using MemberAssigendTask.Model;
using Microsoft.EntityFrameworkCore;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
       
        public DbSet<AssigningTask> AssigningTask { get; set; }
      
    }
}
