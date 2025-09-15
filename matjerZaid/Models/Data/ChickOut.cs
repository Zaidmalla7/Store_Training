using ECApp.Model.Data;
using ECApp.Model.Database;
using StoreOn.Models;
using System.ComponentModel.DataAnnotations;

namespace ECApp.Model.Data
{
    public class ChickOut
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This  filed is required")]

        public string ShippingAddress { get; set; } = null!;


    }
}
