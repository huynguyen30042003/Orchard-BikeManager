namespace BikeManagerV3.Customer.DTOs.VehicleOwnerships;

public class VehicleOwnershipQuery
{
    public Guid? CustomerId { get; set; }

    public bool? IsCurrentOwner { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}