using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using RabbitMQSignalRConsumer.Data;
using RabbitMQSignalRConsumer.Models;
using RabbitMQSignalRConsumer.Service;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace RabbitMQSignalRConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TimerService _timerService;
        private readonly IUser _user;
        private readonly ICompanySubsciption _companySubscription;
        private readonly ICompanyMaster _companyMaster;

        private HubConnection _connection;
        private const string HubUrl = "~/messageHub";

        public HomeController(ILogger<HomeController> logger, TimerService timerService, IUser user, ICompanySubsciption companySubscription, ICompanyMaster companyMaster)
        {
            _logger = logger;
            _timerService = timerService;
            _user = user;
            _companySubscription = companySubscription;
            _companyMaster = companyMaster;
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
                    Dictionary<string, string> _querystringdata = new Dictionary<string, string>();
                    _querystringdata.Add("userid", obj.UserID.ToString());
                    _connection = new HubConnection(HubUrl, _querystringdata);
                    TempData["SignalRHubUrl"] = @"http://localhost:6842/messageHub?userid=" + obj.UserID.ToString();
                    TempData["userID"] = obj.UserID; 
                  //  List<UserCompanySubscription> lst = await _companySubscription.GetCompanybyUserID(obj.UserID);
                  //  //    if (lst != null || lst.Count > 0)
                    //    {
                    //        for (int i = 0; i < lst.Count; i++)
                    //        {
                    //            int threadNumber = i + 1; // Just for display purposes
                    //            Task.Run(() => DoWork(threadNumber));
                    //        }
                    //    }


                  //  _timerService.Start(lst, obj.UserID.ToString());
                    //   return Redirect("~/Index.html");
                    return RedirectToAction("SignalR");
                }
            }

            return View();
        }
        public async Task  <IActionResult> SignalR()
        {
            int userid =Convert.ToInt32( TempData["userID"]);
            List<UserCompanySubscription> lst = await _companySubscription.GetCompanybyUserID(userid);
            List<int> lstCompanyIDs = lst.Select(x => x.CompanyID).ToList();
          
            List<CompanyDetail> companyDetails = new List<CompanyDetail>();
            foreach(int x in lstCompanyIDs)
            {
                CompanyDetail obj=new CompanyDetail();
             CompanyMaster c=   await _companyMaster.GetCompanyByID(x);
                obj.CompanyID = c.CompanyID;
                obj.CompanyName= c.CompanyName;
                obj.CompanyCode = c.CompanyCode;
                companyDetails.Add(obj);
            }
            MessageHub.AddOrUpdateUserConnection(userid.ToString(), "", lstCompanyIDs,  companyDetails);
            _timerService.Start(lstCompanyIDs, userid.ToString());
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