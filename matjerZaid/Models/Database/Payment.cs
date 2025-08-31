namespace ECApp.Model.Database
{
    public class Payment
    {

        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public int PaymentMethodId { get; set; }

        public int StatusId { get; set; }

        public DateOnly CreatedAt { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual PaymentMethod PaymentMethod { get; set; } = null!;

        public virtual Status Status { get; set; } = null!;
    }
}
