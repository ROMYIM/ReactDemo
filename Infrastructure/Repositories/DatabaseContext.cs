using System;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models;
using ReactDemo.Domain.Models.System;

namespace ReactDemo.Infrastructure.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating  (ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.Username).HasName("username").IsUnique();
        }
    }
}