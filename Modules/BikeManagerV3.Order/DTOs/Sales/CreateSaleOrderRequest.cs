namespace BikeManagerV3.Order.DTOs.Sales;

public class CreateSaleOrderRequest
{
    public CustomerInfoRequest Customer { get; set; } = new();

    public string PaymentMethod { get; set; } = "Cash";

    public List<CreateSaleOrderItemRequest> Items { get; set; } = new();
}