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
    public class JiraContext : IdentityDbContext
    {
        public DbSet<Ticket> Bugs { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public JiraContext(DbContextOptions<JiraContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ticket>()
                .HasOne(b => b.AppUser)
                .WithMany(u => u.Bugs)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
        
        
    }
}
