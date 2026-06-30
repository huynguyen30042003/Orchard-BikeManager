
namespace BikeManagerV3.Product.Models;

public class ProductVariant
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string SKU { get; set; } = default!;

    public string? Color { get; set; }

    public string? Battery { get; set; }

    public string? MotorPower { get; set; }

    public decimal ImportPrice { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal WholesalePrice { get; set; }

    public int? StockQuantity { get; set; } = 0;

    public int WarrantyMonths { get; set; }

    public bool TrackSerial { get; set; }

    // navigation
    public ProductModels Product { get; set; } = default!;

    public ICollection<SerialNumber> SerialNumbers
        = new List<SerialNumber>();
}