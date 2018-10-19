using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models.Party;

namespace ReactDemo.Infrastructure.Reporistories
{
    public class DatabaseContext : DbContext
    {
        
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Member> Member { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
    }
}