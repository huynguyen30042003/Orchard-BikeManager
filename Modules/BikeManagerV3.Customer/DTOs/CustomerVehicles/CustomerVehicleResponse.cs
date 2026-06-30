// DTOs/CustomerVehicles/CustomerVehicleResponse.cs
namespace BikeManagerV3.Customer.DTOs.CustomerVehicles;

public class CustomerVehicleResponse
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid BrandId { get; set; }

    public string ModelName { get; set; }
        = string.Empty;

    public string PlateNumber { get; set; }
        = string.Empty;

    public string FrameNumber { get; set; }
        = string.Empty;

    public string EngineNumber { get; set; }
        = string.Empty;

    public string BatterySerial { get; set; }
        = string.Empty;

    public DateTime PurchaseDate { get; set; }
}