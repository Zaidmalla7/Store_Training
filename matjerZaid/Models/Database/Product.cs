namespace ECApp.Model.Database
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int? Stock { get; set; }

        public int CategoryId { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string? Sku { get; set; }

        public DateOnly CreatedAt { get; set; }

        public DateOnly? UpdatedAt { get; set; }

        public int? StatusId { get; set; }

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<Inventorymovement> inventorymovements { get; set; } = new List<Inventorymovement>();


        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual Status? Status { get; set; }

        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    }
}
