namespace ECApp.Model.Data
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public string StatusName { get; set; }
        public string? UserName { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }

        public List<ProductDetailsViewModel> Products { get; set; }
    }
}
