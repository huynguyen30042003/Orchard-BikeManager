using BikeManagerV3.Product.DTOs.Brand;
using BikeManagerV3.Product.DTOs.Category;
using BikeManagerV3.Product.Enums;

namespace BikeManagerV3.Product.Product.DTOs;

public class ProductSimpleDto
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Guid BrandId { get; set; }

    public string SKU { get; set; } = string.Empty;

    public string? Barcode { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Slug { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? ThumbnailUrl { get; set; }

    public ProductType ProductType { get; set; }
        = ProductType.Bicycle;

    // navigation dto
    public CategoryDto? Category { get; set; }

    public BrandDto? Brand { get; set; }
}