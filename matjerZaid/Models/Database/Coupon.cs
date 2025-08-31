namespace ECApp.Model.Database
{
    public class Coupon
    {
        public int CouponId { get; set; }

        public string Code { get; set; } = null!;

        public int Discount { get; set; }

        public DateOnly ExpirationDate { get; set; }

        public decimal MinOrderValue { get; set; }

        public int UsageLimit { get; set; }
    }
}
