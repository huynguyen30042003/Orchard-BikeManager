using BikeManagerV3.Suppliers.Enum;

namespace BikeManagerV3.Suppliers.DTOs.PurchaseOrder
{
    public class PurchaseOrderQuery
    {
        public string? Keyword { get; set; }

        public Guid? SupplierId { get; set; }

        public Guid? WarehouseId { get; set; }

        public PurchaseOrderStatus? Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; } = "CreatedAt";

        public bool Descending { get; set; } = true;
    }
}
