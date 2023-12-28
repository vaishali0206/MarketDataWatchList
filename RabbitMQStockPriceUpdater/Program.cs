// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQStockPriceUpdater.Data;
using Microsoft.EntityFrameworkCore;
using RabbitMQStockPriceUpdater.Service;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System;

Console.WriteLine("Hello, World!");
var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);



var config = builder.Build();

var connectionString = config["ConnectionStrings:StudentDbContext"];



using IHost host = CreateHostBuilder(args).Build();



IHostBuilder CreateHostBuilder(string[] strings)
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddDbContext<CompanyDbContext>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<ICompanyDbContext, CompanyDbContext>();
            services.AddScoped<ICompanyQueue, CompanyQueue>();
            services.AddScoped<IPriceUpdater, PriceUpdater>();
            services.AddScoped<Random>();
        });


}

// create a service scope
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;
//var timer = new System.Threading.Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
//async Task TimerCallback(object? state)
//{
//    await ExecutePeriodicTaskAsync();
//}
while (true)
{
    Console.WriteLine("Starts");
    await ExecutePeriodicTaskAsync();
    await Task.Delay(TimeSpan.FromSeconds(10));
}
async Task ExecutePeriodicTaskAsync()
{
   
    // This method will be executed every 10 seconds
    // Call the service method here
    var priceUpdater = services.GetRequiredService<IPriceUpdater>();
    var companyQueue = services.GetRequiredService<ICompanyQueue>();

    List<UserCompanySubscription> lst = await companyQueue.SubscriptionList();
    int companyId = await companyQueue.GetRandomCompanyID(lst);
    decimal price = await priceUpdater.RandomePriceGenerator(companyId);
    CompanyPrice obj = new CompanyPrice();
    obj.CompanyID = companyId;
    obj.Price = price;
    await priceUpdater.UpdateDatabaseAsync(obj);



    // await priceUpdater.UpdateDataPeriodicallyAsync(); // Replace YourServiceMethod with the actual method you want to call

    // Console.WriteLine("Executing periodic task...");
}