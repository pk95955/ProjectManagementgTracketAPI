using Microsoft.EntityFrameworkCore;
using ProjectManagementTracketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.DbContexts
{
    public class ApplicationDbContexts: DbContext
    {
        public ApplicationDbContexts(DbContextOptions<ApplicationDbContexts> options): base(options)
        {
                
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<SkillsMaster> SkillsMaster { get; set; }
        public DbSet<SkillsTransaction> SkillsTransaction { get; set; }
        public DbSet<AssigningTask> AssigningTask { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SkillsTransaction>()
                .HasKey(o => new { o.SkillId, o.MemberId });
        }
    }
}
