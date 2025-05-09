namespace matjerZaid.Models.Database
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
