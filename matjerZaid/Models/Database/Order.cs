using StoreOn.Models;

namespace matjerZaid.Models.Database
{
    public class Order
    {
       
            public int OrderId { get; set; }

            public int StatusId { get; set; }
            public string UserId { get; set; }  // المفتاح الأجنبي (FK)
            public ApplicationUser User { get; set; }  // العلاقة مع المستخدم

            public decimal TotalPrice { get; set; }

            public DateTime CreatedAt { get; set; } // تغيير DateOnly إلى DateTime

            public int? CouponId { get; set; }

            public string ShippingAddress { get; set; } = null!;

            public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
            public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
            public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

            public virtual Status Status { get; set; } = null!;
        }

 
}
