namespace BikeManagerV3.Product.DTOs.ProductVariant;

public class ProductVariantQuery
{
    public string? Search { get; set; }

    public string? SearchBy { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public bool? TrackSerial { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
