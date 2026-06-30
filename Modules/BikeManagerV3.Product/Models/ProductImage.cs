
namespace BikeManagerV3.Product.Models;

public class ProductImage
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; }
        = default!;

    public int SortOrder { get; set; }

    public bool IsThumbnail { get; set; }

    public ProductModels Product { get; set; }
        = default!;
}