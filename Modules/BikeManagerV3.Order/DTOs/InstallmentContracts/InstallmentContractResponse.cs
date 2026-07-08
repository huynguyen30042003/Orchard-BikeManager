using BikeManagerV3.Order.DTOs.InstallmentProviders;
using BikeManagerV3.Order.DTOs.Orders;

namespace BikeManagerV3.Order.DTOs.InstallmentContracts;

public class InstallmentContractResponse
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProviderId { get; set; }

    public string ContractNumber { get; set; }
        = string.Empty;

    public decimal LoanAmount { get; set; }

    public decimal DownPayment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int InstallmentMonths { get; set; }

    public decimal MonthlyPayment { get; set; }

    public decimal InterestRate { get; set; }

    public string ContractStatus { get; set; }
        = string.Empty;
    public InstallmentProviderResponse? InstallmentProvider { get; set; }
    public OrderResponse? Order { get; set; }

}