namespace BikeManagerV3.Suppliers.DTOs.PurchaseOrder
{
    public class ReceivedSerialRequest
    {
        public Guid ProductVariantId { get; set; }

        public string? SerialCode { get; set; } = default!;
        public string? FrameNumber { get; set; } = null!;

        public string? EngineNumber { get; set; }

        public string? BatterySerial { get; set; }

        public string? MotorSerial { get; set; }
    }
}
