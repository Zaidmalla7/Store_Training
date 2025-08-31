using ECApp.Model.Database;
using StoreOn.Models;

namespace ECApp.Model.Data
{
    public class CardModel
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }
        public string UserId { get; set; }  // هذا عبارة عن المفتاح الخارجي (Foreign Key)
        public ApplicationUser User { get; set; }  // هذا الـ Navigation Property

        public int Quantity { get; set; }

        public DateOnly AddedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
