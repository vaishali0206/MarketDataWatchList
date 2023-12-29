using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Configuration;
using SignalRConsumer.Models;
using SignalRConsumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<RabbitMQConfiguration>();
builder.Services.Configure<RabbitMQConfiguration>(
    builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
builder.Services.AddSingleton<IMessageConsumer, RabbitMQMessageConsumer>();
builder.Services.AddSingleton<IHubContext<MessageHub>>();

builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
//app.MapHub<MessageHub>("/messageHub");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MessageHub>("/messageHub");
});
app.Use(async (context, next) =>
{
    var hubContext = context.RequestServices
                            .GetRequiredService<IHubContext<MessageHub>>();
    //...

    if (next != null)
    {
        await next.Invoke();
    }
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ReceiveMessages}/{id?}");

app.Run();
