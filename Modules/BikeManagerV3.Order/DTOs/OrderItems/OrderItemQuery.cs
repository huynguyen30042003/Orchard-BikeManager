// DTOs/OrderItems/OrderItemQuery.cs
namespace BikeManagerV3.Order.DTOs.OrderItems;

public class OrderItemQuery
{
    public Guid? OrderId { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}