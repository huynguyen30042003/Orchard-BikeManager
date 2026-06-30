using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.DTOs.ProductImage;

public class UpdateProductImageRequest
{
    [Required]
    public Guid ProductId { get; set; }

    public IFormFile? Image { get; set; }

    public int SortOrder { get; set; }

    public bool IsThumbnail { get; set; }
}