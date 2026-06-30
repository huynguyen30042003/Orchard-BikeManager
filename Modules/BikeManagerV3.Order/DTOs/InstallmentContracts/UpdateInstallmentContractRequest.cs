// DTOs/InstallmentContracts/UpdateInstallmentContractRequest.cs
namespace BikeManagerV3.Order.DTOs.InstallmentContracts;

public class UpdateInstallmentContractRequest
{
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
}