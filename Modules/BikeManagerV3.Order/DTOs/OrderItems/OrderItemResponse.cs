using BikeManagerV3.Order.DTOs.Orders;
using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.DTOs.SerialNumber;
using BikeManagerV3.Product.Models;

namespace BikeManagerV3.Order.DTOs.OrderItems;

public class OrderItemResponse
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductVariantId { get; set; }

    public Guid? SerialNumberId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalPrice { get; set; }
    public OrderResponse Order { get; set; }
        = null!;
    public ProductVariantResponse ProductVariant { get; set; }
        = null!;
    public SerialNumberResponse SerialNumber { get; set; }
        = null!;
}