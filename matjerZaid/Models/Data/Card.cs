using ECApp.Model.Database;
using StoreOn.Models;

namespace ECApp.Model.Data
{
    public class Card
    {
        public int CartId { get; set; }

        // Product info
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }      // السعر بعد الخصم (أو نفسه إن ما في خصم)


        // Quantity
        public int Quantity { get; set; }

        // Primary image
        public string ImageUrl { get; set; }
    }
}
