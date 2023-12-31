using System.ComponentModel.DataAnnotations;

namespace RabbitMQSignalRConsumer.Data
{
    public class CompanyMaster
    {
        [Key]
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
    }
}
