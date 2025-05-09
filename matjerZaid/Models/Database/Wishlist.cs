using StoreOn.Models;

namespace matjerZaid.Models.Database
{
    public class Wishlist
    {
        public int WishlistId { get; set; }

        public int ProductId { get; set; }
        public string UserId { get; set; }  // هذا عبارة عن المفتاح الخارجي (Foreign Key)
        public ApplicationUser User { get; set; }  // هذا الـ Navigation Property

        public DateOnly AddedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
