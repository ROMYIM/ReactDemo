using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models.User;
using ReactDemo.Infrastructure.Entities;
using ReactDemo.Infrastructure.Event.Buses;

namespace ReactDemo.Infrastructure.Repositories
{
    public class DatabaseContext : DbContext
    {
        private readonly LocalEventBus _eventBus;

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, LocalEventBus eventBus) : base(options)
        {
            _eventBus = eventBus;
        }

        protected override void OnModelCreating  (ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.Username).HasName("username").IsUnique();
            builder.Entity<UserRole>().HasKey(ur => new { ur.UserID, ur.RoleID });
            // builder.Entity<UserRole>().HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserID);
            // builder.Entity<UserRole>().HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleID);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                var entity = entry.Entity as IGenerateDomainEvents;
                if (entity != null)
                {
                    var events = entity.DomainEvents;
                    
                }
            }
            return base.SaveChangesAsync();
        }
    }
}