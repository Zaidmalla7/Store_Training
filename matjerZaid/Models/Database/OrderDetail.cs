namespace ECApp.Model.Database
{
    public class OrderDetail
    {
        public int OrderDetailsId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Subtotal { get; set; }

        public decimal DiscountApplied { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
