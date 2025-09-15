using ECApp.Data;
using ECApp.Model.Data;
using ECApp.Model.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EUROPIECE.Controllers
{
    public class CardController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardController1(ApplicationDbContext context)
        {
            _context = context;
        }
        // عرض السلة
        [Authorize]

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                    .ThenInclude(p => p.ProductImages)
                .Select(c => new Card
                {
                    ProductId = c.ProductId,
                    CartId = c.CartId,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    Quantity = c.Quantity,
                    ImageUrl = c.Product.ProductImages
                        .Where(img => img.IsPrimary)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault() ?? "/images/default.png",
                    FinalPrice = (c.Product.DiscountPrice.HasValue && c.Product.DiscountPrice.Value > 0)
    ? c.Product.DiscountPrice.Value
    : c.Product.Price,

                })
                .ToListAsync();

            var vm = new CartCheckoutVM
            {
                CartItems = cartItems,
                CheckoutInfo = new ChickOut()
            };
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Json(new { success = false, message = "User is not logged in." });

            var userId = userIdClaim.Value;

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found." });

            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
                existingCartItem.Quantity += quantity;
            else
                await _context.Carts.AddAsync(new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                });

            await _context.SaveChangesAsync();

            // احسب عدد العناصر في السلة
            var cartCount = await _context.Carts.CountAsync(c => c.UserId == userId);

            return Json(new { success = true, message = "Product has been added to your cart.", count = cartCount });
        }


        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var item = await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.CartId == id);

            if (item == null) return NotFound();

            item.Quantity++;
            await _context.SaveChangesAsync();
            var finalPrice = (item.Product.DiscountPrice.HasValue && item.Product.DiscountPrice.Value > 0)
                ? item.Product.DiscountPrice.Value
                : item.Product.Price; var subtotal = finalPrice * item.Quantity;

            return Json(new
            {
                quantity = item.Quantity,
                subtotal = subtotal,
                finalPrice = finalPrice
            });
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            var item = await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.CartId == id);

            if (item == null || item.Quantity <= 1)
                return BadRequest();

            item.Quantity--;
            await _context.SaveChangesAsync();

            var finalPrice = (item.Product.DiscountPrice.HasValue && item.Product.DiscountPrice.Value > 0)
                ? item.Product.DiscountPrice.Value
                : item.Product.Price; var subtotal = finalPrice * item.Quantity;

            return Json(new
            {
                quantity = item.Quantity,
                subtotal = subtotal,
                finalPrice = finalPrice
            });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == id && c.UserId == userId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Json(new { count = 0 });
            }

            var userId = userIdClaim.Value;

            var count = await _context.Carts
     .Where(c => c.UserId == userId)
     .CountAsync();


            return Json(new { count = count });
        }

    }
}
