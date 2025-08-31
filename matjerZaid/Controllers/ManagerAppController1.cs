using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECApp.Controllers
{
    public class ManagerAppController1 : Controller
    {
        public IActionResult Manager()
        {
            return View();
        }
    }
}
