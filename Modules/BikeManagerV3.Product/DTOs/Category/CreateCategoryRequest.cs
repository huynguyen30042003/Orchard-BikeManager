using Microsoft.AspNetCore.Http;

namespace BikeManagerV3.Product.DTOs.Category;

public class CreateCategoryRequest
{
    public Guid? ParentId { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public IFormFile? Image { get; set; }

    public string? Description { get; set; }
}