using StoreOn.Models;

namespace ECApp.Model.Data
{
    public class Orders
    {
        public int OrderId { get; set; }

        public int StatusId { get; set; }
        public string? UserName { get; set; }  

        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } // تغيير DateOnly إلى DateTime


        public string ShippingAddress { get; set; } = null!;
        public string? StatusName { get; set; }

    }
}
