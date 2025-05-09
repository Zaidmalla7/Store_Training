using System.ComponentModel.DataAnnotations;

namespace matjerZaid.Models.Data
{
    public class Podects
    {
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]
        public string Name { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]

        public string Description { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]

        public decimal Price { get; set; }

        public int? Stock { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]

        public int CategoryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]

        public decimal? DiscountPrice { get; set; }

        public string? Sku { get; set; }

        public DateOnly CreatedAt { get; set; }

        public DateOnly? UpdatedAt { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]

        public int? StatusId { get; set; }
        public string? CategoryName { get; set; }
        
        public string? StatusName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]

        public string? ImageUrl { get; set; } = null!;
        public bool AutoCreateInventory { get; set; }




    }
}
