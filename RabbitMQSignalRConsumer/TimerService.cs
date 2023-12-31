using Microsoft.Extensions.Hosting;
using RabbitMQSignalRConsumer;
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
    public void Start()
    {
        // Implement your timer start logic here
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    // Rest of the TimerService implementation...

    //private void DoWork(object state)
    //{
    //    // Implement the work to be done by the timer
    //}
    private void DoWork(object state)
    {
        _rabbitMQConsumer.StartConsuming();
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
