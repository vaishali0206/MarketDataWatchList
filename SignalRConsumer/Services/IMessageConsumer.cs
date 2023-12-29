namespace SignalRConsumer.Services
{
   
    public interface IMessageConsumer
    {
        void StartReceiving(Action<string> onMessageReceived);
    }
}
