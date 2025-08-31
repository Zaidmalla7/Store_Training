namespace ECApp.Model.Database
{
    public class Status
    {
        public int StatusId { get; set; }

        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
