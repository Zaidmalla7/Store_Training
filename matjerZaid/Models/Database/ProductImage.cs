namespace ECApp.Model.Database
{
    public class ProductImage
    {
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public DateOnly? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
