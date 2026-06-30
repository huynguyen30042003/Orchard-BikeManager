using BikeManagerV3.Product.Models;

namespace BikeManagerV3.Suppliers.Models
{
    public class PurchaseOrderItem
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Guid ProductVariantId { get; set; }

        public string ProductVariantSKU { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public bool TrackSerial { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; } = null!;
    }
}
