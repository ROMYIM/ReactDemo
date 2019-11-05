using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models.User;
using ReactDemo.Infrastructure.Domain;
using ReactDemo.Infrastructure.Event.Helpers;

namespace ReactDemo.Infrastructure.Repositories
{
    public class DatabaseContext : DbContext
    {
        private readonly IEventHelper _eventHelper;

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IEventHelper eventHelper) : base(options)
        {
            _eventHelper = eventHelper;
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
            var entries = ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                var entity = entry.Entity as IGenerateDomainEvents;
                if (entity != null)
                {
                    var events = entity.DomainEvents;
                    foreach (var @event in events)
                    {
                        if (_eventHelper.HandleEventData(@event))
                        {
                            _eventHelper.Push(@event);
                        }
                    }
                }
            }
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
                    foreach (var @event in events)
                    {
                        if (_eventHelper.HandleEventData(@event))
                        {
                            _eventHelper.PushAsync(@event);
                        }
                    }
                }
            }
            return base.SaveChangesAsync();
        }
    }
}