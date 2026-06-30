namespace BikeManagerV3.Suppliers.DTOs.PurchaseOrderItem
{
    public class CreatePurchaseOrderItemRequest
    {
        public Guid ProductVariantId { get; set; }

        public int Quantity { get; set; }

        public bool? TrackSerial { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
