namespace BikeManagerV3.Order.Models;

public class InstallmentContract
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProviderId { get; set; }

    public string ContractNumber { get; set; }
        = string.Empty;

    public decimal LoanAmount { get; set; }

    public decimal DownPayment { get; set; }

    public int InstallmentMonths { get; set; }

    public decimal MonthlyPayment { get; set; }

    public decimal InterestRate { get; set; }

    public string ContractStatus { get; set; }
        = string.Empty;

    // Navigation
    public Order Order { get; set; }
        = null!;

    public InstallmentProvider Provider { get; set; }
        = null!;
}