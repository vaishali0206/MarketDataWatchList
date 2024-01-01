using Microsoft.AspNetCore.Mvc;
using RabbitMQSignalRConsumer.Data;
using RabbitMQSignalRConsumer.Models;
using RabbitMQSignalRConsumer.Service;
using System.Collections.Generic;
using System.Diagnostics;

namespace RabbitMQSignalRConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TimerService _timerService;
        private readonly IUser _user;
        private readonly ICompanySubsciption _companySubscription;


        public HomeController(ILogger<HomeController> logger, TimerService timerService, IUser user, ICompanySubsciption companySubscription)
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
        public async Task<IActionResult> Index(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                Users obj = await _user.GetUserByUserName(username);
                if (obj != null)
                {
                    List<UserCompanySubscription> lst = await _companySubscription.GetCompanybyUserID(obj.UserID);
                    //    if (lst != null || lst.Count > 0)
                    //    {
                    //        for (int i = 0; i < lst.Count; i++)
                    //        {
                    //            int threadNumber = i + 1; // Just for display purposes
                    //            Task.Run(() => DoWork(threadNumber));
                    //        }
                    //    }


                    _timerService.Start(lst);
                    return Redirect("~/Index.html");
                }
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