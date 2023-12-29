
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalRConsumer.Models;
using SignalRConsumer.Services;
using System.Diagnostics;
using System.Text;

namespace SignalRConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageConsumer _messageConsumer;

        public HomeController(IMessageConsumer messageConsumer, ILogger<HomeController> logger)
        {
            _messageConsumer = messageConsumer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReceiveMessages()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}