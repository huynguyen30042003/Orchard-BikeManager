using BikeManagerV3.Product.Product.DTOs;

namespace BikeManagerV3.Product.DTOs.ProductVariant;

public class ProductVariantResponse
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

    public int? StockQuantity { get; set; }

    public int WarrantyMonths { get; set; }

    public bool TrackSerial { get; set; }

    public ProductSimpleDto? Product { get; set; }

}