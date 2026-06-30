using BikeManagerV3.Warranty.DTOs.Warranties;
using BikeManagerV3.Warranty.Responses;

namespace BikeManagerV3.Warranty.Services.Interfaces;

public interface IWarrantyService
{
    Task<PagedResult<WarrantyDto>> GetAllAsync(WarrantyQuery request);

    Task<WarrantyDto?> GetByIdAsync(Guid id);

    Task<WarrantyDto> CreateAsync(CreateWarrantyDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateWarrantyDto dto);

    Task<bool> DeleteAsync(Guid id);
}