using Microsoft.EntityFrameworkCore;
using RabbitMQStockPriceUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public class PriceUpdater : IPriceUpdater
    {
        // private readonly ICompanyDbContext _context;
        private readonly ICompanyDbContextFactory _contextFactory;
        private readonly Random _random;

        public PriceUpdater(ICompanyDbContextFactory contextFactory, Random random)
        {
            _contextFactory = contextFactory;
            _random = random;
        }

        public async Task<decimal> RandomePriceGenerator(int CompanyID)
        {
            decimal randomDecimal = (decimal)_random.NextDouble() * 1000;
            return await Task.FromResult(randomDecimal);
        }
        public async Task<CompanyPrice> GetCompanyID(int CompanyID)
        {

            using (var context = _contextFactory.CreateContext())
            {
                CompanyPrice companyPrice = await context.CompanyPrice.Where(x => x.CompanyID == CompanyID).FirstOrDefaultAsync();
                //  _context.Detach(companyPrice);
                return companyPrice;
            }
            

        }
        public async Task<CompanyPrice> UpdateDatabaseAsync(CompanyPrice companyPrice)
        {
            using (var context = _contextFactory.CreateContext())
            {
                CompanyPrice obj = await GetCompanyID(companyPrice.CompanyID);
                if (obj != null)
                    companyPrice.PriceID = obj.PriceID;
               context.AddOrUpdate(companyPrice);
            }
            return companyPrice;

        }
    }
}
