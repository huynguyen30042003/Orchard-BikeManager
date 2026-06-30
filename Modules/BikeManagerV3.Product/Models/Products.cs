using BikeManagerV3.Product.Enums;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.Models;

public class ProductModels
{
    public Guid Id { get; set; }

    // Category = loại sản phẩm
    public Guid CategoryId { get; set; }

    // Brand = hãng
    public Guid BrandId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SKU { get; set; } = default!;

    [MaxLength(100)]
    public string? Barcode { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    public string Slug { get; set; } = default!;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? ThumbnailUrl { get; set; }

    public ProductType ProductType { get; set; }
        = ProductType.Bicycle;

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Category Category { get; set; }
        = default!;

    public Brand Brand { get; set; }
        = default!;

    public ICollection<ProductVariant> ProductVariants
        = new List<ProductVariant>();

    public ICollection<ProductImage> Images
        = new List<ProductImage>();
}