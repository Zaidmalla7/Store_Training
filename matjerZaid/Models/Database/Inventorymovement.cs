using StoreOn.Models;
using System.ComponentModel.DataAnnotations;

namespace ECApp.Model.Database
{
    public class Inventorymovement
    {
        [Key] 
        public int movementId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public string movementType { get; set; } = null!;
        public string? note { get; set; }
        public DateTime createdAt { get; set; }
        public string? createdBy { get; set; }
        public ApplicationUser User { get; set; }

        public virtual Product Product { get; set; } = null!;

    }
}
