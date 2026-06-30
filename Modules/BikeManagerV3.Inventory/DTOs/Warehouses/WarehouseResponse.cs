public class WarehouseResponse
{
    public Guid Id { get; set; }

    public Guid BranchId { get; set; }

    public object? Branch { get; set; }

    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }
}