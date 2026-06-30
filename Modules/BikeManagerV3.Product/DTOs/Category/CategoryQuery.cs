namespace BikeManagerV3.Product.DTOs.Category;

public class CategoryQuery
{
    public string? Search { get; set; }

    public Guid? ParentId { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}