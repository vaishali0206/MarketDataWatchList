using Microsoft.EntityFrameworkCore;
using RabbitMQSignalRConsumer;
using RabbitMQSignalRConsumer.Data;
using RabbitMQSignalRConsumer.Service;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
           .Build();

var defaultConnectionString = config.GetConnectionString("ConnectionStrings:StudentDbContext");



var connectionString = config["ConnectionStrings:StudentDbContext"];
builder.Services .AddDbContext<CompanyDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddScoped<ICompanyDbContext, CompanyDbContext>();
builder.Services.AddSingleton<RabbitMQConsumer>();
builder.Services.AddSingleton<TimerService>();
builder.Services.AddScoped<IUser,User>();
builder.Services.AddScoped<ICompanySubsciption,CompanySubsciption>();
builder.Services.AddScoped<ICompanyMaster, CompanyMasterDetails>();
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
