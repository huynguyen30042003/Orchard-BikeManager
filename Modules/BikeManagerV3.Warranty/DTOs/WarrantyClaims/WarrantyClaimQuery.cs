namespace BikeManagerV3.Warranty.DTOs.WarrantyClaims
{
    public class WarrantyClaimQuery
    {
        public Guid? WarrantyId { get; set; }

        public Guid? RepairOrderId { get; set; }

        public string? Status { get; set; }
    }
}
