using BikeManagerV3.Product.Enums;

namespace BikeManagerV3.Product.DTOs.Product;

public class ProductQuery
{
    // SEARCH
    public string? Search { get; set; }

    // FILTERS
    public Guid? CategoryId { get; set; }

    public Guid? BrandId { get; set; }

    public bool? IsPublished { get; set; }

    public ProductType ProductType { get; set; }
    = ProductType.Bicycle;

    // SORT
    public string? SortBy { get; set; }
        = "createdAt";

    public string? SortOrder { get; set; }
        = "desc";

    // PAGINATION
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}