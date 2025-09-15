namespace ECApp.Model.Data
{
    public class ProductDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Sku { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }    
        public decimal Subtotal { get; set; }   
        public string ImageUrl { get; set; }
    }
}
