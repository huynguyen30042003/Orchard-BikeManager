// DTOs/InstallmentContracts/InstallmentContractResponse.cs
using BikeManagerV3.Order.DTOs.InstallmentProviders;

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

    public int InstallmentMonths { get; set; }

    public decimal MonthlyPayment { get; set; }

    public decimal InterestRate { get; set; }

    public string ContractStatus { get; set; }
        = string.Empty;
    public InstallmentProviderResponse? InstallmentProvider { get; set; }

}