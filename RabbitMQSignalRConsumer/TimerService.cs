using Microsoft.Extensions.Hosting;
using RabbitMQSignalRConsumer;
using RabbitMQSignalRConsumer.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

public class TimerService :  IDisposable
{
    private Timer _timer;
    private readonly RabbitMQConsumer _rabbitMQConsumer;

    public TimerService(RabbitMQConsumer rabbitMQConsumer)
    {
        _rabbitMQConsumer = rabbitMQConsumer;
    }

    //public Task StartAsync(CancellationToken stoppingToken)
    //{
    //    _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

    //    return Task.CompletedTask;
    //}
    public void Start(List<int> lstCompanyIDs, string userId)
    {
          //  List<int> lstCompanyIDs = lst.Select(x => x.CompanyID).ToList();
            for (int i = 0; i < 5; i++)
            {
              //  int threadNumber = i + 1; // Just for display purposes
                Task.Run(() => DoWork(lstCompanyIDs, userId));
            }
        
        // Implement your timer start logic here
       // _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    // Rest of the TimerService implementation...

    //private void DoWork(object state)
    //{
    //    // Implement the work to be done by the timer
    //}
    private void DoWork1(object state)
    {
       // _rabbitMQConsumer.StartConsuming();
    }
    private void DoWork(List<int> lstCompanyIDs, string userId)
    {
        while (true)
        {
            _rabbitMQConsumer.StartConsuming(lstCompanyIDs, userId);
            Thread.Sleep(100);
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
