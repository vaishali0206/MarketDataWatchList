using Microsoft.AspNetCore.Mvc;
using RabbitMQSignalRConsumer.Data;
using RabbitMQSignalRConsumer.Models;
using RabbitMQSignalRConsumer.Service;
using System.Diagnostics;

namespace RabbitMQSignalRConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TimerService _timerService;
        private readonly User _user;
        private readonly CompanySubsciption _companySubscription;


        public HomeController(ILogger<HomeController> logger, TimerService timerService, User user, CompanySubsciption companySubscription)
        {
            _logger = logger;
            _timerService = timerService;
            _user = user;
            _companySubscription = companySubscription;
        }

        public IActionResult Index()
        {
              return View();
           
        }
        [HttpPost]
        public async Task< IActionResult> Index(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                Users obj =await _user.GetUserByUserName(username);
                if(obj!=null )
                {
             List<UserCompanySubscription> lst = await _companySubscription.GetCompanybyUserID(obj.UserID);
                    if(lst!=null  || lst.Count>0)
                    {
                    
                    }
                }

                _timerService.Start();
                return Redirect("~/Index.html");
            }
           
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