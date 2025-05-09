using StoreOn.Models;

namespace matjerZaid.Models.Database
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int ProductId { get; set; }

        public int Rating { get; set; }
        public string UserId { get; set; }  // هذا عبارة عن المفتاح الخارجي (Foreign Key)
        public ApplicationUser User { get; set; }  // هذا الـ Navigation Property

        public string? Review1 { get; set; }

        public DateOnly CreatedAt { get; set; }

        public int StatusId { get; set; }

        public virtual Product Product { get; set; } = null!;

        public virtual Status Status { get; set; } = null!;
    }
}
