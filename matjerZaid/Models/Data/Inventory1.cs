using StoreOn.Models;

namespace ECApp.Model.Data
{
    public class Inventory1
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int StatusId { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string Note { get; set; }= null!;
        public int MinimumStock { get; set; }
        public string? StatusName{ get; set; }


        public string UpdatedBy { get; set; }
        public ApplicationUser User { get; set; }  // هذا الـ Navigation Property
    }
}
