using BikeManagerV3.Product.Enums;

namespace BikeManagerV3.Product.DTOs.SerialNumber;

public class SerialNumberQuery
{
    public string? Search { get; set; }

    public string? SearchBy { get; set; }

    public Guid? ProductVariantId { get; set; }

    public string? SerialCode { get; set; }

    public CurrentStatus? CurrentStatus { get; set; }

    public Guid? WarehouseId { get; set; }

    public bool? TrackSerial { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

}