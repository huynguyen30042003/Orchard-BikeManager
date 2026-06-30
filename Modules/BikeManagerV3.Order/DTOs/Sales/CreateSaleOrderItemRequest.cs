namespace BikeManagerV3.Order.DTOs.Sales;

public class CreateSaleOrderItemRequest
{
    public Guid ProductVariantId { get; set; }

    public Guid? SerialNumberId { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }
}