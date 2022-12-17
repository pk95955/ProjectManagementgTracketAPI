using Microsoft.EntityFrameworkCore;
using ProjectManagementTracketAPI.Models;
namespace ProjectManagementTracketAPI.DbContexts
{
    public class ApplicationDbContexts: DbContext
    {
        public ApplicationDbContexts(DbContextOptions<ApplicationDbContexts> options): base(options){}
      
        public DbSet<User> User { get; set; }
       
    }
}
