using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RabbitMQStockPriceUpdater.Data
{
    public interface ICompanyDbContext : IDisposable
    {

        DbSet<UserCompanySubscription> UserCompanySubscription { get; set; }
        public DbSet<CompanyPrice> CompanyPrice { get; set; }
        void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync();

    }
}
