using ClientDemo.Hubs;
using ClientDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using V9AgentInfo.Hubs;
using V9AgentInfo.Models;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Services;
using static ClientDemo.Helper.Helper;

namespace ClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotifyServices _NotifyServices;
        private readonly IHubContext<NotificationHub, INotifyHubClient> _hubContext;
        private readonly V9Context _Context;
        APINotify _api = new APINotify();

        public HomeController(ILogger<HomeController> logger, INotifyServices NotifyServices, IHubContext<NotificationHub, INotifyHubClient> hubContext, V9Context Context)
        {
            _logger = logger;
            _NotifyServices = NotifyServices;
            _hubContext = hubContext;
            _Context = Context;

        }

        public IActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Get()
        {

            var item = await _NotifyServices.GetAll();
            var notify = await _Context.Notifys.ToListAsync();
            await _hubContext.Clients.All.BroadcastNotify(notify);
            return View(item);

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var infos = new List<Notify>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Notify/AgentNotify/GetAll");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                infos = JsonConvert.DeserializeObject<List<Notify>>(result);
            }
            return View(infos);
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
