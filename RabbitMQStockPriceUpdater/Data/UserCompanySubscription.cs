using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Data
{
    public class UserCompanySubscription
    {
        [Key]
        public int SubscriptionID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
    }
}
