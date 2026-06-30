using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.DTOs.ProductImage;

public class CreateProductImageRequest
{
    [Required]
    public Guid ProductId { get; set; }

    public IFormFile Image { get; set; }
        = default!;

    public int SortOrder { get; set; }

    public bool IsThumbnail { get; set; }
}