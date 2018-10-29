using System;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models;
using ReactDemo.Domain.Models.Conference;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Models.System;

namespace ReactDemo.Infrastructure.Repositories
{
    public class DatabaseContext : DbContext
    {
        
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Hall> Halls { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
    }
}