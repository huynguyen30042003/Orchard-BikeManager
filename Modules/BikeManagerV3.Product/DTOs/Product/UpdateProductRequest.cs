using BikeManagerV3.Product.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.DTOs.Product;

public class UpdateProductRequest
{
    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    public Guid BrandId { get; set; }

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

    public IFormFile? Thumbnail { get; set; }

    public ProductType ProductType { get; set; }
        = ProductType.Bicycle;

    public bool IsPublished { get; set; }
}