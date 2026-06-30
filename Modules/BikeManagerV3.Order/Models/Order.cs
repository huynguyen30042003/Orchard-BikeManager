namespace BikeManagerV3.Order.Models;

public class Order
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
        = DateTime.UtcNow;

    // Navigation
    public ICollection<OrderItem> Items { get; set; }
        = new List<OrderItem>();

    public ICollection<InstallmentContract> InstallmentContracts
    { get; set; } = new List<InstallmentContract>();
}