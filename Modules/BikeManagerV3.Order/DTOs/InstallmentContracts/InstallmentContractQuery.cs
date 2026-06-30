// DTOs/InstallmentContracts/InstallmentContractQuery.cs
namespace BikeManagerV3.Order.DTOs.InstallmentContracts;

public class InstallmentContractQuery
{
    public Guid? ProviderId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? OrderId { get; set; }

    public string? ContractStatus { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}