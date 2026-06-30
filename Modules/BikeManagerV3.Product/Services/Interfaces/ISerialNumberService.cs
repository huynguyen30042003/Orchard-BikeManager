using BikeManagerV3.Product.DTOs.SerialNumber;
using BikeManagerV3.Product.Responses;

namespace BikeManagerV3.Product.Services.Interfaces;

public interface ISerialNumberService
{
    Task<PagedResult<SerialNumberResponse>> GetAll(
        SerialNumberQuery query);

    Task<ApiResponse<object>> GetById(
        Guid id);

    Task<ApiResponse<object>> CreateSerial(
        CreateSerialNumberRequest request);

    Task<ApiResponse<object>> Update(
        Guid id,
        UpdateSerialNumberRequest request);

    Task<ApiResponse<object>> Delete(
        Guid id);

}