using ECApp.Model.Data;
using ECApp.Model.Database;
using StoreOn.Models;

namespace ECApp.Model.Data
{ 
public class CartCheckoutVM
    {
        public List<Card> CartItems { get; set; } = new();
        public ChickOut CheckoutInfo { get; set; } = new();
    }
}
