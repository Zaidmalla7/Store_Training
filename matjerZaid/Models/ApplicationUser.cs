using matjerZaid.Models.Database;
using Microsoft.AspNetCore.Identity;

namespace StoreOn.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime CreateAt { get; set; }
        // إضافة العلاقة مع السلة
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        // ربط المستخدم بالطلبات (One-to-Many)
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        // العلاقة مع التقييمات (One-to-Many)
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        // العلاقة مع الـ Wishlist (One-to-Many)
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
        public ICollection<Inventorymovement> inventorymovement { get; set; } = new List<Inventorymovement>();

    }
}
