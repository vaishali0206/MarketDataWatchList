using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSignalRConsumer.Data
{
    public class CompanyDbContext : DbContext, ICompanyDbContext
    {
        public CompanyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserCompanySubscription> UserCompanySubscription { get; set; }
        public DbSet<CompanyPrice> CompanyPrices { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<CompanyMaster> CompanyMaster { get; set; }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        public void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                // If the entity is not being tracked, try to find it in the context
                var existingEntity = Set<TEntity>().Find(GetKeyValues(entity));

                if (existingEntity == null)
                {
                    Set<TEntity>().Add(entity);
                }
                else
                {
                    // If the entity is found, update its properties
                    // Entry(existingEntity).CurrentValues.SetValues(entity);
                    var entryExisting = Entry(existingEntity);
                    foreach (var property in entryExisting.OriginalValues.Properties)
                    {
                        var original = entryExisting.OriginalValues[property];
                        var current = entry.CurrentValues[property];

                        // Only update properties that have changed
                        if (!object.Equals(original, current))
                        {
                            entryExisting.Property(property).IsModified = true;
                            entryExisting.Property(property).CurrentValue = current;
                        }
                    }
                }
            }

            SaveChanges();
        }

        private object[] GetKeyValues<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Entry(entity);
            var keyProperties = entry.Metadata.FindPrimaryKey().Properties;

            return keyProperties.Select(p => entry.Property(p.Name).CurrentValue).ToArray();
        }

    }
}
