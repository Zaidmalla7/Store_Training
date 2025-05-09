using StoreOn.Models;

namespace matjerZaid.Models.Database
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public DateOnly UpdatedAt { get; set; }

        public int StatusId { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string? Note { get; set; }
        
            public string UpdatedBy { get; set; }
        public ApplicationUser User { get; set; }  // هذا الـ Navigation Property

        public int MinimumStock { get; set; }

        public virtual Product Product { get; set; } = null!;

        public virtual Status Status { get; set; } = null!;
    }
}
