using BikeManagerV3.Order.Data;
using BikeManagerV3.Order.DTOs.InstallmentContracts;
using BikeManagerV3.Order.DTOs.InstallmentProviders;
using BikeManagerV3.Order.Models;
using BikeManagerV3.Order.Responses;
using BikeManagerV3.Order.Services;
using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.Product.DTOs;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace BikeManagerV3.Order.Services;

public class InstallmentContractService
    : IInstallmentContractService
{
    private readonly OrderDbContext _context;

    public InstallmentContractService(
        OrderDbContext context)
    {
        _context = context;
    }

    public async Task<
        ApiResponse<
            InstallmentContractResponse>>
        CreateAsync(
            CreateInstallmentContractRequest request)
    {
        var contract =
            new InstallmentContract
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                ProviderId =
                    request.ProviderId,
                ContractNumber =
                    request.ContractNumber,
                LoanAmount =
                    request.LoanAmount,
                DownPayment =
                    request.DownPayment,
                InstallmentMonths =
                    request.InstallmentMonths,
                MonthlyPayment =
                    request.MonthlyPayment,
                InterestRate =
                    request.InterestRate,
                ContractStatus =
                    request.ContractStatus
            };

        _context.InstallmentContracts
            .Add(contract);

        await _context.SaveChangesAsync();

        return ApiResponse<
            InstallmentContractResponse>.Ok(
                Map(contract),
                "Created successfully");
    }

    public async Task<
        PagedResult<
                InstallmentContractResponse>>
        GetAllAsync(
            InstallmentContractQuery query)
    {
        var dbQuery = _context
            .InstallmentContracts
            .Include(x => x.Order)
            .Include(x => x.Provider)
            .AsQueryable();
        if (query.CustomerId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.Order.CustomerId ==
                query.CustomerId.Value);
        }
        if (query.OrderId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.OrderId ==
                query.OrderId.Value);
        }
        if (query.ProviderId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.ProviderId ==
                query.ProviderId.Value);
        }

        if (!string.IsNullOrWhiteSpace(
            query.ContractStatus))
        {
            dbQuery = dbQuery.Where(x =>
                x.ContractStatus ==
                query.ContractStatus);
        }
        var totalItems = await dbQuery.CountAsync();

        var contracts = await dbQuery
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .Select(contract => new InstallmentContractResponse
            {
                Id = contract.Id,
                OrderId = contract.OrderId,
                ProviderId = contract.ProviderId,
                ContractNumber = contract.ContractNumber,
                LoanAmount = contract.LoanAmount,
                DownPayment = contract.DownPayment,
                InstallmentMonths = contract.InstallmentMonths,
                MonthlyPayment = contract.MonthlyPayment,
                InterestRate = contract.InterestRate,
                ContractStatus = contract.ContractStatus,
                InstallmentProvider = new InstallmentProviderResponse
                {
                    Id = contract.Provider.Id,
                    Name = contract.Provider.Name,
                    Phone = contract.Provider.Phone,
                    ApiEndpoint = contract.Provider.ApiEndpoint,
                    IsActive = contract.Provider.IsActive,
                }
            })
            .ToListAsync();

        return new PagedResult<InstallmentContractResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),
            Items = contracts
        };
    }
    public async Task<
       ApiResponse<
           InstallmentContractResponse>>
       GetByOrderIdAsync(Guid orderId)
    {
        var contract = await _context
            .InstallmentContracts
            .Include(x => x.Provider)
            .FirstOrDefaultAsync(x =>
                x.OrderId == orderId);

        if (contract == null)
        {
            return ApiResponse<
                InstallmentContractResponse>
                .Fail(
                    "Contract not found");
        }
        var result = new InstallmentContractResponse
        {
            Id = contract.Id,
            OrderId = contract.OrderId,
            ProviderId =
                contract.ProviderId,
            ContractNumber =
                contract.ContractNumber,
            LoanAmount =
                contract.LoanAmount,
            DownPayment =
                contract.DownPayment,
            InstallmentMonths =
                contract.InstallmentMonths,
            MonthlyPayment =
                contract.MonthlyPayment,
            InterestRate =
                contract.InterestRate,
            ContractStatus =
                contract.ContractStatus,
            InstallmentProvider = new InstallmentProviderResponse
            {
                Id = contract.Provider.Id,
                Name = contract.Provider.Name,
                Phone = contract.Provider.Phone,
                ApiEndpoint = contract.Provider.ApiEndpoint,
                IsActive = contract.Provider.IsActive,
            }
        };
        return ApiResponse<
            InstallmentContractResponse>
            .Ok(result);
    }

    public async Task<
        ApiResponse<
            InstallmentContractResponse>>
        GetByIdAsync(Guid id)
    {
        var contract = await _context
            .InstallmentContracts
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (contract == null)
        {
            return ApiResponse<
                InstallmentContractResponse>
                .Fail(
                    "Contract not found");
        }

        return ApiResponse<
            InstallmentContractResponse>
            .Ok(Map(contract));
    }

    public async Task<
        ApiResponse<
            InstallmentContractResponse>>
        UpdateAsync(
            Guid id,
            UpdateInstallmentContractRequest request)
    {
        var contract = await _context
            .InstallmentContracts
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (contract == null)
        {
            return ApiResponse<
                InstallmentContractResponse>
                .Fail(
                    "Contract not found");
        }

        contract.ProviderId =
            request.ProviderId;

        contract.ContractNumber =
            request.ContractNumber;

        contract.LoanAmount =
            request.LoanAmount;

        contract.DownPayment =
            request.DownPayment;

        contract.InstallmentMonths =
            request.InstallmentMonths;

        contract.MonthlyPayment =
            request.MonthlyPayment;

        contract.InterestRate =
            request.InterestRate;

        contract.ContractStatus =
            request.ContractStatus;

        await _context.SaveChangesAsync();

        return ApiResponse<
            InstallmentContractResponse>
            .Ok(
                Map(contract),
                "Updated successfully");
    }

    public async Task<
        ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var contract = await _context
            .InstallmentContracts
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (contract == null)
        {
            return ApiResponse<string>
                .Fail(
                    "Contract not found");
        }

        _context.InstallmentContracts
            .Remove(contract);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static
        InstallmentContractResponse Map(
            InstallmentContract contract)
    {
        return new InstallmentContractResponse
        {
            Id = contract.Id,
            OrderId = contract.OrderId,
            ProviderId =
                contract.ProviderId,
            ContractNumber =
                contract.ContractNumber,
            LoanAmount =
                contract.LoanAmount,
            DownPayment =
                contract.DownPayment,
            InstallmentMonths =
                contract.InstallmentMonths,
            MonthlyPayment =
                contract.MonthlyPayment,
            InterestRate =
                contract.InterestRate,
            ContractStatus =
                contract.ContractStatus
        };
    }
}