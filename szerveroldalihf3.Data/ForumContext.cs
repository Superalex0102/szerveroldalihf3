using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szerveroldalihf3.Entities.Entity;

namespace szerveroldalihf3.Data
{
    public class ForumContext : IdentityDbContext
    {
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public ForumContext(DbContextOptions<ForumContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bug>()
                .HasOne(b => b.AppUser)
                .WithMany(u => u.Bugs)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
        
        
    }
}
