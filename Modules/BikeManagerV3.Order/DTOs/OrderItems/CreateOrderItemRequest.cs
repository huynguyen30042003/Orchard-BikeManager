// DTOs/OrderItems/CreateOrderItemRequest.cs
namespace BikeManagerV3.Order.DTOs.OrderItems;

public class CreateOrderItemRequest
{
    public Guid OrderId { get; set; }

    public Guid ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalPrice { get; set; }
}