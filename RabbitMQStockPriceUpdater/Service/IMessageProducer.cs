using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(T message);
        
    }
}
