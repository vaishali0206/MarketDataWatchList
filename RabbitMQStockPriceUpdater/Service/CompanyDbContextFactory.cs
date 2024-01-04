using Microsoft.EntityFrameworkCore;
using RabbitMQStockPriceUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public class CompanyDbContextFactory : ICompanyDbContextFactory
    {
        private readonly DbContextOptions<CompanyDbContext> _options;

        public CompanyDbContextFactory(DbContextOptions<CompanyDbContext> options)
        {
            _options = options;
        }

        public ICompanyDbContext CreateContext()
        {
            return new CompanyDbContext(_options);
        }
    }

}
