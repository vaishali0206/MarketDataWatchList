using RabbitMQSignalRConsumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSignalR();
builder.Services.AddSingleton<RabbitMQConsumer>();
builder.Services.AddSingleton<TimerService>();
//builder.Services.AddHostedService<TimerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.MapHub<MessageHub>("/messageHub");
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub< MessageHub>("/messageHub");
//   // endpoints.MapFallbackToFile("Index.html");
//});
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
