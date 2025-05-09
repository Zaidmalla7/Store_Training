using System.ComponentModel.DataAnnotations;

namespace matjerZaid.Models.Data
{
    public class Imge
    {
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public DateOnly? UpdatedAt { get; set; }

        public DateOnly CreatedAt { get; set; }

    }
}
