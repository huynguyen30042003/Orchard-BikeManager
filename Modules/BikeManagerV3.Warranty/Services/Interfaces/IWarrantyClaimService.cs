// Services/IWarrantyClaimService.cs
using BikeManagerV3.Warranty.DTOs.WarrantyClaims;

namespace BikeManagerV3.Warranty.Services.Interfaces;

public interface IWarrantyClaimService
{
    Task<IEnumerable<WarrantyClaimDto>> GetAllAsync(WarrantyClaimQuery request);

    Task<WarrantyClaimDto?> GetByIdAsync(Guid id);

    Task<WarrantyClaimDto> CreateAsync(CreateWarrantyClaimDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateWarrantyClaimDto dto);

    Task<bool> DeleteAsync(Guid id);
}