using Microsoft.AspNetCore.Mvc;
using ECApp.Data;
using ECApp.Model.Data;
using ECApp.Model.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EUROPIECE.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = userIdClaim.Value;

            var wishlistItems = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                    .ThenInclude(p => p.ProductImages)
                .Select(w => new ECApp.Model.Data.Wishlis
                {
                    WishlistId = w.WishlistId,
                    ProductName = w.Product.Name,
                    Price = w.Product.Price,
                    ImageUrl = w.Product.ProductImages
                        .Where(img => img.IsPrimary)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault() ?? "/images/default.png",
                     FinalPrice = w.Product.DiscountPrice ??w.Product.Price, // 👈 خصم إن وجد
                })
                .ToListAsync();

            return View(wishlistItems);
        }

        // إضافة للمفضلة
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = userIdClaim.Value;

            var exists = await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);

            if (exists)
                return BadRequest("المنتج موجود بالفعل في المفضلة");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("المنتج غير موجود");

            var wishlistItem = new Wishlist
            {
                ProductId = productId,
                UserId = userId,
                AddedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            await _context.Wishlists.AddAsync(wishlistItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // حذف من المفضلة
        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = userIdClaim.Value;

            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.WishlistId == id && w.UserId == userId);

            if (wishlistItem == null)
                return NotFound();

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // عدد عناصر المفضلة
        [HttpGet]
        public async Task<IActionResult> GetWishlistCount()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Json(new { count = 0 });

            var userId = userIdClaim.Value;

            var count = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .CountAsync();

            return Json(new { count = count });
        }
    }
}
