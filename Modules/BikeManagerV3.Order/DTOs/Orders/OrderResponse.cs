// DTOs/Orders/OrderResponse.cs
using BikeManagerV3.Customer.DTOs.Customers;

namespace BikeManagerV3.Order.DTOs.Orders;

public class OrderResponse
{
    public Guid Id { get; set; }

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

    public required string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }
    public CustomerResponse? Customer { get; set; }
}