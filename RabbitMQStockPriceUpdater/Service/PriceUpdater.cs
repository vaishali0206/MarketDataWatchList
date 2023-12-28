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
        private readonly ICompanyDbContext _context;
        private readonly Random _random;

        public PriceUpdater(ICompanyDbContext context)
        {
            _context = context;
            _random = new Random();
        }
        public async Task<decimal> RandomePriceGenerator(int CompanyID)
        {
            decimal randomDecimal = (decimal)_random.NextDouble() * 1000;
            return await Task.FromResult(randomDecimal);
        }
        public async Task<CompanyPrice> GetCompanyID(int CompanyID)
        {
            return await _context.CompanyPrice.Where(x => x.CompanyID == CompanyID).FirstOrDefaultAsync();
           
        }
        public async Task UpdateDatabaseAsync(CompanyPrice companyPrice)
        {
            CompanyPrice obj = await GetCompanyID(companyPrice.CompanyID);
            if(obj != null)
            {
                companyPrice.PriceID = obj.PriceID;
                _context.CompanyPrice.Update(companyPrice);
            }
            else
                _context.CompanyPrice.Add(companyPrice);

          await  _context.SaveChangesAsync();
        }
    }
}
