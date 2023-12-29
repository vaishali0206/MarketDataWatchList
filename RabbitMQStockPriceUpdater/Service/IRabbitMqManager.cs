using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public interface IRabbitMqManager
    {
        void DeclareQueue(string queueName);
        void PublishMessage<T>(string queueName,T message);
        void ConsumeMessages(string queueName, Action<string> messageHandler);
        void CloseConnection();
    }
}
