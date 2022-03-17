using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Models.Entities.AgentInfo;
using V9AgentInfo.Services;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInfoServices _InfoServices;

        public HomeController(IInfoServices InfoServices)
        {
            _InfoServices = InfoServices;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateInfoModel model)
        {
            await _InfoServices.Create(model);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
