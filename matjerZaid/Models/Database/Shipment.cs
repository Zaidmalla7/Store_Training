namespace matjerZaid.Models.Database
{
    public class Shipment
    {
        public int ShipmentId { get; set; }

        public int OrderId { get; set; }

        public int StatusId { get; set; }

        public int TrackingNumber { get; set; }

        public DateOnly? EstimatedDelivery { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
