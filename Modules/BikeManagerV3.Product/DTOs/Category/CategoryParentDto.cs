namespace BikeManagerV3.Product.DTOs.Category;

public class CategoryParentDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;
}