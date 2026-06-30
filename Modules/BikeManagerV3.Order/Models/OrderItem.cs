using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.DTOs.SerialNumber;
using BikeManagerV3.Product.Models;

namespace BikeManagerV3.Order.Models;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductVariantId { get; set; }

    public Guid? SerialNumberId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal CostPrice { get; set; }

    public decimal ProfitAmount { get; set; }
    // Navigation
    public Order Order { get; set; }
        = null!;
}