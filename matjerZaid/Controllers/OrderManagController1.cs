using ECApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Zaid.Controllers
{
    [Route("ManagerAppController1/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class OrderManagController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderManagController1(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OrderManagPage()
        {
          
            var result = await _context.Orders.DefaultIfEmpty().GroupBy(o => 1).Select(d => new
            {
                Pending = _context.Orders.Where(s => s.StatusId == 1004).Count(),
                Confirmed = _context.Orders.Where(s => s.StatusId == 1005).Count(),
                Shipped = _context.Orders.Where(s => s.StatusId == 1007).Count(),
                OutforDelivery = _context.Orders.Where(s => s.StatusId == 1008).Count(),
                Delivered = _context.Orders.Where(s => s.StatusId == 1009).Count()

        }).FirstAsync();
             ViewBag.Pending = result.Pending;
             ViewBag.Confirmed = result.Confirmed;
             ViewBag.Shipped = result.Shipped;
             ViewBag.OutforDelivery = result.OutforDelivery;
             ViewBag.Deliveryred = result.Delivered;
            

            var orders = await _context.Orders
                                 .OrderByDescending(o => o.CreatedAt)
                                 .Select(o => new ECApp.Model.Data.Orders
                                 {
                                    OrderId = o.OrderId,
                                    ShippingAddress = o.ShippingAddress,
                                    StatusName = o.Status.Name,
                                    UserName = o.User.UserName,
                                    CreatedAt = o.CreatedAt,
                                    TotalPrice = o.TotalPrice
                                 }).ToListAsync();



            return View(orders);
        }

        public async Task<IActionResult> DetailsOrder(int orderId)
        {

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.ProductImages)
                .Include(o => o.Status)
                .Include(p => p.User)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound();
            var viewModel = new ECApp.Model.Data.OrderDetailsViewModel
            {
                OrderId = order.OrderId,
                StatusName = order.Status.Name,
                UserName = order.User.UserName,
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Products = order.OrderDetails.Select(od => new ECApp.Model.Data.ProductDetailsViewModel
                {
                    ProductId = od.ProductId,
                    Sku = od.Product.Sku,
                    ProductName = od.Product.Name,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    Subtotal = od.Quantity * od.Price,
                    ImageUrl = od.Product.ProductImages.FirstOrDefault() != null
                      ? od.Product.ProductImages.FirstOrDefault().ImageUrl
                : "/images/no-image.png"
                }).ToList()
            };

            return Json(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string statusId)
        {
            var order = await _context.Orders.Include(o => o.Status)
                                             .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return Json(new { success = false });

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == statusId);
            if (status == null) return Json(new { success = false });

            order.StatusId = status.StatusId;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int Id)
        {
            var order = await _context.Orders.Where(o => o.OrderId == Id).FirstOrDefaultAsync();
            if(order == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }
            var orderde = await _context.OrderDetails.Where(d => d.OrderId == Id).ToListAsync();
            if(orderde != null)
            {
                _context.OrderDetails.RemoveRange(orderde);
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order deleted successfully!" });


        }
    }
}
