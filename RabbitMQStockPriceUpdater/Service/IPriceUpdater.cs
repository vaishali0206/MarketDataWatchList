using RabbitMQStockPriceUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public interface IPriceUpdater
    {
        Task<decimal> RandomePriceGenerator(int CompanyID);
        public Task<CompanyPrice> UpdateDatabaseAsync(CompanyPrice obj);
       // Task UpdateDataPeriodicallyAsync(CancellationToken cancellationToken);
    }
}
