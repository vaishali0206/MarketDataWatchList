using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Data
{
    public class CompanyDbContext : DbContext, ICompanyDbContext
    {
        public CompanyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserCompanySubscription> UserCompanySubscription { get; set; }
        public DbSet<CompanyPrice> CompanyPrice { get; set; }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        //public void AddOrUpdate<TEntity>(TEntity entity)
        //where TEntity : class
        //{


        //    var entry = Entry(entity);

        //    if (entry.State == EntityState.Detached)
        //    {
        //        Set<TEntity>().Add(entity);
        //    }
        //    else
        //    {
        //        entry.State = EntityState.Modified;
        //    }

        //    SaveChangesAsync();
        //}
    }
}
