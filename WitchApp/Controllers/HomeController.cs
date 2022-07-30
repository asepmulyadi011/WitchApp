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
        #region Properties
        private readonly ILogger<HomeController> _logger;
        private List<VillagerModel> dataVillagers;
        private List<int> dataKill;
        #endregion

        #region Public Method
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _InitialSession();
            _Average();

            return View(dataVillagers);
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
        public IActionResult AddVillager(VillagerModel villager)
        {
            _InitialSession();

            if (!_Validate(villager))
            {
                _SetAlert("fail");

                return View();
            }

            villager.Year = villager.YearOfDeath - villager.AgeOfDeath;
            _UpdateKill(villager.Year);
            villager.NumberOfPeopleKilled = dataKill.Take(villager.Year).Sum();
            dataVillagers.Add(villager);

            SessionHelper.SetObjectAsJson(HttpContext.Session, "villagers", dataVillagers);

            _Average();
            _SetAlert("success");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region Private Method
        private void _InitialSession()
        {
            dataVillagers = SessionHelper.GetObjectFromJson<List<VillagerModel>>(HttpContext.Session, "villagers");

            if (dataVillagers == null)
            {
                dataVillagers = new List<VillagerModel>();
            }

            dataKill = SessionHelper.GetObjectFromJson<List<int>>(HttpContext.Session, "kills");

            if (dataKill == null)
            {
                dataKill = new List<int>();
                dataKill.Add(1);
                dataKill.Add(1);
            }
        }

        private void _UpdateKill(int year)
        {
            int start = dataKill.Count() - 1;
            int end = year - 1;

            for (int i = start; i < end; i++)
            {
                int firstNumber = dataKill.ElementAt(i);
                int secondNumber = dataKill.ElementAt(i - 1);
                int total = firstNumber + secondNumber;

                dataKill.Add(total);
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "kills", dataKill);
        }

        private bool _Validate(VillagerModel villager)
        {
            if(villager.AgeOfDeath < 0 || villager.YearOfDeath < 0)
            {
                return false;
            }
            else if(villager.YearOfDeath - villager.AgeOfDeath <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void _Average()
        {
            if(dataVillagers.Count() > 0)
            {
                float totalKill = dataVillagers.Select(x => x.NumberOfPeopleKilled).Sum();
                float dataAmount = dataVillagers.Count();
                float average = totalKill / dataAmount;
                
                ViewBag.Average = String.Format("{0:0.00}", average);
            }
            else
            {
                ViewBag.Average = 0;
            }
        }

        private void _SetAlert(string type)
        {
            switch(type)
            {
                case "success":
                    ViewBag.Display = true;
                    ViewBag.Message = "Data saved successfully";
                    ViewBag.Type = "alert-success";

                    break;
                case "fail":
                    ViewBag.Display = true;
                    ViewBag.Message = "Error -1: Invalid data";
                    ViewBag.Type = "alert-danger";

                    break;
                default:
                    ViewBag.Display = true;
                    ViewBag.Message = "Error: Data Not Found";
                    ViewBag.Type = "alert-warning";

                    break;
            }
        }
        #endregion
    }
}
