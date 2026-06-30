
namespace BikeManagerV3.Suppliers.DTOs.Supplier
{
    public class UpdateSupplierRequest
    {
        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? TaxCode { get; set; }

        public string? ContactPerson { get; set; }

        public bool IsActive { get; set; }
    }
}
