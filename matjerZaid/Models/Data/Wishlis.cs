using ECApp.Model.Database;
using StoreOn.Models;

namespace ECApp.Model.Data
{
    public class Wishlis
    {
        public int WishlistId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }  // هذا عبارة عن المفتاح الخارجي (Foreign Key)
        public ApplicationUser User { get; set; }  // هذا الـ Navigation Property
        public decimal FinalPrice { get; set; }      // السعر بعد الخصم (أو نفسه إن ما في خصم)

        public string ImageUrl { get; set; }

        public DateOnly AddedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
