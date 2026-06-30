using BikeManagerV3.Product.Enums;

namespace BikeManagerV3.Product.DTOs.SerialNumber;

public class CreateSerialNumberRequest
{
    public Guid ProductVariantId { get; set; }

    public string? FrameNumber { get; set; }

    public string? EngineNumber { get; set; }

    public string? BatterySerial { get; set; }

    public string? MotorSerial { get; set; }

    public string? QRCode { get; set; }

    public DateTime? ManufacturingDate { get; set; }

    public DateTime? ImportDate { get; set; }

    public DateTime? WarrantyStart { get; set; }

    public DateTime? WarrantyEnd { get; set; }

    public CurrentStatus? CurrentStatus { get; set; }

    public Guid? WarehouseId { get; set; }
}