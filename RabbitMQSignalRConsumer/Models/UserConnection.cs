namespace RabbitMQSignalRConsumer.Models
{
    public class UserConnection
    {
      
        public string ConnectionID { set; get; }
        public List<int> CompanyIDs { set; get; }
      public  List<CompanyDetail> companyDetails { set; get; }
    }
}
