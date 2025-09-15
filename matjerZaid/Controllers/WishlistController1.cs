using ECApp.Data;
using ECApp.Model.Data;
using ECApp.Model.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EUROPIECE.Controllers
{
    public class WishlistController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController1(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]

        public async Task<IActionResult> Wishlist()
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
                    ProductId = w.ProductId,
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

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Json(new { success = false, message = "User is not logged in." });

            var userId = userIdClaim.Value;

            var exists = await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);

            if (exists)
                return Json(new { success = false, message = "Product already exists in your wishlist." });

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found." });

            await _context.Wishlists.AddAsync(new Wishlist
            {
                ProductId = productId,
                UserId = userId,
                AddedAt = DateOnly.FromDateTime(DateTime.Now)
            });

            await _context.SaveChangesAsync();

            // احسب عدد العناصر في المفضلة
            var wishlistCount = await _context.Wishlists.CountAsync(w => w.UserId == userId);

            return Json(new { success = true, message = "Product has been added to your wishlist.", count = wishlistCount });
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
