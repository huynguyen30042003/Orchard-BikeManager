using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Product.DTOs;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.DTOs.ProductImage;

public class ProductImageQuery
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; }
        = default!;

    public int SortOrder { get; set; }

    public bool IsThumbnail { get; set; }
    public ProductSimpleDto? Product { get; set; }
}