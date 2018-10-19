using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Models.System;

namespace ReactDemo.Infrastructure.Reporistories
{
    public class DatabaseContext : DbContext
    {
        
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<User> User { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
    }
}