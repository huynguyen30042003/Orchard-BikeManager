using BikeManagerV3.Suppliers.DTOs.PurchaseOrderItem;

namespace BikeManagerV3.Suppliers.DTOs.PurchaseOrder
{
    public class PurchaseOrderResponse
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;

        public string SupplierName { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public List<PurchaseOrderItemResponse> Items { get; set; }
            = [];
    }
}
