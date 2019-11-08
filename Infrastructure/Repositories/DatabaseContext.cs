using System;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.Injector;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactDemo.Domain.Models.User;
using ReactDemo.Infrastructure.Domain;
using ReactDemo.Infrastructure.Event.Helpers;

namespace ReactDemo.Infrastructure.Repositories
{
    public class DatabaseContext : DbContext
    {
        private readonly Guid _instanceId;

        [FromContainer]
        public IEventHelper EventHelper { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            _instanceId = Guid.NewGuid();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.Username).HasName("username").IsUnique();
            builder.Entity<UserRole>().HasKey(ur => new { ur.UserID, ur.RoleID });
            // builder.Entity<UserRole>().HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserID);
            // builder.Entity<UserRole>().HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleID);
        }

        public override int SaveChanges()
        {
            PushDomainEvents();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // PushDomainEvents();
            return base.SaveChangesAsync();
        }

        public void PushDomainEvents()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                var entity = entry.Entity as IGenerateDomainEvents;
                if (entity != null)
                {
                    var events = entity.DomainEvents;
                    foreach (var @event in events)
                    {
                        if (EventHelper.HandleEventData(@event))
                        {
                            EventHelper.PushAsync(@event);
                        }
                    }
                }
            }
        }
    }
}