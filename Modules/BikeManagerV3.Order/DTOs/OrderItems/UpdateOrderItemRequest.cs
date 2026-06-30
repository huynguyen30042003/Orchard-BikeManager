// DTOs/OrderItems/UpdateOrderItemRequest.cs
namespace BikeManagerV3.Order.DTOs.OrderItems;

public class UpdateOrderItemRequest
{
    public Guid ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalPrice { get; set; }
}