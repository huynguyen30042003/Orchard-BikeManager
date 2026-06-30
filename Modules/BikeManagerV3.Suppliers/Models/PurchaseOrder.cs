using BikeManagerV3.Suppliers.Enum;
using BikeManagerV3.Suppliers.Models;

namespace BikeManagerV3.Suppliers.Models
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;

        public Guid SupplierId { get; set; }

        public Guid WarehouseId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public string CreatedBy { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? ReceivedAt { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public ICollection<PurchaseOrderItem> Items { get; set; }
            = [];
    }
}
