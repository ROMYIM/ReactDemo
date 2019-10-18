using System;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models.User;

namespace ReactDemo.Infrastructure.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating  (ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.Username).HasName("username").IsUnique();
            builder.Entity<UserRole>().HasKey(ur => new { ur.UserID, ur.RoleID });
            // builder.Entity<UserRole>().HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserID);
            // builder.Entity<UserRole>().HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleID);
        }
    }
}