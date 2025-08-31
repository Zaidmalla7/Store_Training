using ECApp.Data;
using ECApp.Model.Data;
using ECApp.Model.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EUROPIECE.Controllers
{
    public class InventoryController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController1(ApplicationDbContext context)
        {
            _context = context;
        }
        public  async Task<IActionResult> Inventory()
        {
            List<ECApp.Model.Data.Inventory1> inventories = new List<ECApp.Model.Data.Inventory1>();
            inventories = (from obj in await _context.Inventories.ToListAsync()
                           join obj2 in await _context.Statuses.Where(x => x.Type == "Inventory").ToListAsync() on obj.StatusId equals obj2.StatusId
                           join obj3 in _context.Users on obj.UpdatedBy equals obj3.Id
                           select new ECApp.Model.Data.Inventory1
                           {
                              InventoryId = obj.InventoryId,
                               ProductId = obj.ProductId,
                               Quantity = obj.Quantity,
                               UpdatedAt = obj.UpdatedAt,
                               StatusName= obj2.Name,
                               CreatedAt = obj.CreatedAt,
                               Note = obj.Note,
                               MinimumStock = obj.MinimumStock,
                               UpdatedBy = obj3.FirstName + " " + obj3.LastName,

                           }).ToList();

            return View(inventories);
        }

        public async Task<ActionResult> GetinvoById(int id) {
            var inventory = await _context.Inventories.Where(x => x.InventoryId == id).FirstOrDefaultAsync();

            if (inventory == null)
            {
                return NotFound();
            }
            return Json(new
            {
                id = inventory.InventoryId,
                quantity = inventory.Quantity ,
                minimumStock =  inventory.MinimumStock ,
                note = inventory.Note
            });
        }

        public async Task<ActionResult> EditInventory(Inventory1 inventory1)
        {
            var inv = await _context.Inventories.Where(x => x.InventoryId == inventory1.InventoryId).FirstOrDefaultAsync();

            if(inv == null)
            {
                return NotFound();
            }

            int status = ECApp.Services.InventoryService.GetInventoryStatus(inventory1.Quantity, inventory1.MinimumStock);
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            inv.Quantity = inventory1.Quantity;
            inv.StatusId = status;
            inv.UpdatedBy = userIdClaim?.Value;
            inv.Note = inventory1.Note;
            inv.UpdatedAt = DateTime.Now;
            inv.MinimumStock = inventory1.MinimumStock;


            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<ActionResult> Deleted(int Id)
        {
            var inv = await _context.Inventories.FirstOrDefaultAsync(x => x.InventoryId == Id);
            if (inv == null)
            {
                return NotFound();
            }

            _context.Inventories.Remove(inv);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        public async Task<ActionResult> Details(int id)
        {
            var producet = await _context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();

            if(producet == null)
            {
                return NotFound();
            }
            var status = await _context.Statuses.Where(s => s.StatusId == producet.StatusId).Select(s => s.Name).FirstOrDefaultAsync();
            var catagory = await _context.Categories.Where(c => c.CategoryId == producet.CategoryId).Select(c => c.Name).FirstOrDefaultAsync();
            var img = await _context.ProductImages.Where(i => i.IsPrimary == true).Select(i => i.ImageUrl).FirstOrDefaultAsync();
            return Json(new
            {
                name =  producet.Name,
                description =  producet.Description,
                statuname = status,
                catagoryname = catagory,
                price = producet.Price,
                sku = producet.Sku ,
                discountprice = producet.DiscountPrice,
                createdat = producet.CreatedAt,
                imgurl = img
            });
        }
        public ActionResult account()
        {
            return View();
        }
    }
}
