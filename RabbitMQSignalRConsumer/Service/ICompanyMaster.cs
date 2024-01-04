using RabbitMQSignalRConsumer.Data;

namespace RabbitMQSignalRConsumer.Service
{
    public interface ICompanyMaster
    {
        Task<CompanyMaster> GetCompanyByID(int id);
    }
}
