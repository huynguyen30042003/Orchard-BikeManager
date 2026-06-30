using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.DTOs.brand;

public class UpdateBrandRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    public string Slug { get; set; } = default!;

    public IFormFile? Logo { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    public bool IsActive { get; set; }
}