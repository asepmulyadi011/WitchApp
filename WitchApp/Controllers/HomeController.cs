using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WitchApp.Helpers;
using WitchApp.Models;

namespace WitchApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddVillager()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddVillager(Villager villager)
        {
            List<Villager> dataVillagers = SessionHelper.GetObjectFromJson<List<Villager>>(HttpContext.Session, "villagers");

            if (dataVillagers == null)
            {
                dataVillagers = new List<Villager>();
            }

            dataVillagers.Add(villager);

            SessionHelper.SetObjectAsJson(HttpContext.Session, "villagers", dataVillagers);

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
