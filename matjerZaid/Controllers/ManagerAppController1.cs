using ECApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ManagerAppController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerAppController1(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Manager()
        {
            var result = await _context.Products
    .Select(p => 1) // نعمل select ثابت
    .DefaultIfEmpty() // حتى لو فاضي يرجع صف واحد
    .Select(_ => new {
        ProductCount = _context.Products.Count(),
        UserCount = _context.Users.Count(),
        OrderCount = _context.Orders.Count(),
        InventoryCount = _context.Inventory.Count()
    })
    .FirstAsync();

            ViewBag.Product = result.ProductCount;
            ViewBag.User = result.UserCount;
            ViewBag.Order = result.OrderCount;
            ViewBag.Inventory = result.InventoryCount;


            return View();
        }
    }
}
