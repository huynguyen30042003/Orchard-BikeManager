namespace BikeManagerV3.Inventory.DTOs;

public class UpdateInventoryStockRequest
{
    public int Quantity { get; set; }

    public int ReservedQuantity { get; set; }
}