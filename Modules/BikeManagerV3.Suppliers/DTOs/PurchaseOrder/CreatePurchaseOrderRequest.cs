using BikeManagerV3.Suppliers.DTOs.PurchaseOrderItem;

namespace BikeManagerV3.Suppliers.DTOs.PurchaseOrder
{
    public class CreatePurchaseOrderRequest
    {
        public Guid SupplierId { get; set; }

        public Guid WarehouseId { get; set; }

        public decimal DiscountAmount { get; set; }

        public List<CreatePurchaseOrderItemRequest> Items { get; set; }
            = [];
    }
}
