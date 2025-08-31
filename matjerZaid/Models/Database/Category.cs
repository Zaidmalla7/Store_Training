namespace ECApp.Model.Database
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public DateOnly? CreatedAt { get; set; }

        public int? ParentId { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
