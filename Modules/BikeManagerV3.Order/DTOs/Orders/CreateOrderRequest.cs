namespace BikeManagerV3.Order.DTOs.Orders;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }

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

    public required string CreatedBy { get; set; }
}