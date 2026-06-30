using BikeManagerV3.Product.Models;

namespace BikeManagerV3.Suppliers.DTOs.PurchaseOrderItem
{
    public class PurchaseOrderItemResponse
    {
        public Guid ProductVariantId { get; set; }

        public string ProductVariantSKU { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string SKU { get; set; } = null!;

        public int Quantity { get; set; }

        public bool? TrackSerial { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
