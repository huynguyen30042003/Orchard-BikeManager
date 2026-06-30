namespace BikeManagerV3.Order.DTOs.Sales;

public class CreateInstallmentOrderRequest : CreateSaleOrderRequest
{
    public Guid ProviderId { get; set; }

    public decimal DownPayment { get; set; }

    public decimal LoanAmount { get; set; }

    public int InstallmentMonths { get; set; }

    public decimal InterestRate { get; set; }
}