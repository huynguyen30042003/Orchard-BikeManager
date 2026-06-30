// DTOs/Orders/UpdateOrderRequest.cs
namespace BikeManagerV3.Order.DTOs.Orders;

public class UpdateOrderRequest
{
    public Guid CustomerId { get; set; }

    public string OrderCode { get; set; }
        = string.Empty;

    public decimal SubTotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; }
        = string.Empty;

    public string PaymentStatus { get; set; }
        = string.Empty;

    public string OrderStatus { get; set; }
        = string.Empty;
}