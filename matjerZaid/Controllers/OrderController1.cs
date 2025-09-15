using ECApp.Data;
using ECApp.Model.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Security.Claims;

namespace Zaid.Controllers
{
    public class OrderController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController1(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(ECApp.Model.Data.CartCheckoutVM chickOut)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();   
            }

            var userId = userIdClaim.Value;

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return BadRequest("السلة فارغة!");
            }

            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                TotalPrice = 0, 
                ShippingAddress = chickOut.CheckoutInfo.ShippingAddress,
                StatusId = 1004,
            };

            decimal totalPrice = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Orders.Add(order);

                foreach (var item in cartItems)
                {
                    if (item.Product == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"المنتج رقم  غير متاح.");
                    }
                    var productInventory = await _context.Inventory
                        .Where(i => i.ProductId == item.ProductId).FirstOrDefaultAsync();


                    if (productInventory == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"المنتج  غير موجود في المخزون.");
                    }

                    if (productInventory.Quantity < item.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"الكمية المطلوبة غير متوفرة للمنتج");
                    }
                    productInventory.Quantity -= item.Quantity;

                    var inventorymovement = new Inventorymovement
                    {
                        productId = productInventory.ProductId,
                        quantity = item.Quantity,
                        movementType = "Sale",
                        note = "Stock out due to customer order (pending ID)",
                        createdAt = DateTime.Now,
                        createdBy = userId,
                    };

                    decimal priceToUse = (item.Product.DiscountPrice.HasValue && item.Product.DiscountPrice.Value > 0)
       ? item.Product.DiscountPrice.Value
       : item.Product.Price;

                    decimal subtotal = item.Quantity * priceToUse;
                    totalPrice += subtotal;


                    var orderDetail = new OrderDetail
                    {
                        Order = order, 
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = (item.Product.DiscountPrice.HasValue && item.Product.DiscountPrice.Value > 0)
    ? item.Product.DiscountPrice.Value
    : item.Product.Price,
                        Subtotal = subtotal
                    };

                    _context.OrderDetails.Add(orderDetail);
                    _context.Inventory.Update(productInventory);
                    _context.inventorymovements.Add(inventorymovement);
                }

                order.TotalPrice = totalPrice;

                _context.Carts.RemoveRange(cartItems);

                await _context.SaveChangesAsync();

                foreach (var move in _context.inventorymovements.Local.Where(m => m.note.Contains("pending ID")))
                {
                    move.note = $"Stock out due to customer order #{order.OrderId}";
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction("OrderSuccess", new { orderId = order.OrderId });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IActionResult> OrderSuccess(int orderId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = userIdClaim.Value;

            var order = await _context.Orders
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductImages)
                .Include(o => o.Status)
        .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }


        public async Task<IActionResult> TrackYourOrder(int orderId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = userIdClaim.Value;

            var order = await _context.Orders
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductImages)
                .Include(o => o.Status)
        .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [Authorize]

        public async Task<IActionResult> MyOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = userIdClaim.Value;

            var order = await _context.Orders
                .Where(x => x.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(im => im.ProductImages)
                .Include(o => o.Status)
                .ToListAsync();
            return View(order);

        }
    }
}
