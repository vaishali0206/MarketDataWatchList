using Microsoft.EntityFrameworkCore;
using RabbitMQSignalRConsumer.Data;
using RabbitMQSignalRConsumer.Models;

namespace RabbitMQSignalRConsumer.Service
{
    public class CompanyMasterDetails:ICompanyMaster
    {
        private readonly ICompanyDbContext _context;

        public CompanyMasterDetails(ICompanyDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyMaster> GetCompanyByID(int id)
        {
            return await _context.CompanyMaster.Where(x=>x.CompanyID == id).FirstOrDefaultAsync();
        }

    }
}
