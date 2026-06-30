namespace BikeManagerV3.Suppliers.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? TaxCode { get; set; }

        public string? ContactPerson { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
            = [];
    }
}
