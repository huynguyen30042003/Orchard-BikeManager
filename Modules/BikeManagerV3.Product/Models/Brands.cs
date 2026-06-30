using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.Models;

public class Brand
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    public string Slug { get; set; } = default!;

    public string? LogoUrl { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public ICollection<ProductModels> Products
        = new List<ProductModels>();
}