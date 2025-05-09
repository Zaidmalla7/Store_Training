using System.Diagnostics;
using matjerZaid.Models;
using Microsoft.AspNetCore.Mvc;

namespace matjerZaid.Controllers
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
            var data = new List<dynamic> {
        new { ID = 1, Name = "????", Age = 25 },
        new { ID = 2, Name = "????", Age = 30 },
        new { ID = 3, Name = "????", Age = 28 }
    };

            ViewBag.dataSource = data;
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
