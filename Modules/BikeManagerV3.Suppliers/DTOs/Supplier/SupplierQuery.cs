namespace BikeManagerV3.Suppliers.DTOs.Supplier
{
    public class SupplierQuery
    {
        public string? Keyword { get; set; }

        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; } = "Name";

        public bool Descending { get; set; } = false;
    }
}
