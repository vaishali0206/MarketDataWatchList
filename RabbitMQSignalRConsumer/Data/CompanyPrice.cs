using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSignalRConsumer.Data
{
    public class CompanyPrice
    {
        [Key]
        public int PriceID { get; set; }
        public decimal Price { get; set; }
        public int CompanyID { get; set; }
    }
}
