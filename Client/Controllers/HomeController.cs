using Client.Models;
using Microsoft.AspNetCore.Mvc;
using V9AgentInfo.Services;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
