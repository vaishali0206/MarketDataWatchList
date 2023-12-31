using System.ComponentModel.DataAnnotations;

namespace RabbitMQSignalRConsumer.Data
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
