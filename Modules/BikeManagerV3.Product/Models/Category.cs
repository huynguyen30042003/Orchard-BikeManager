using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Product.Models;

public class Category
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    public string Slug { get; set; } = default!;

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Parent category
    public Category? Parent { get; set; }

    // Child categories
    public ICollection<Category> Children
        = new List<Category>();

    // PRODUCTS
    public ICollection<ProductModels> Products
        = new List<ProductModels>();
}