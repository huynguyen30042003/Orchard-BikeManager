// Services/CustomerStatisticService.cs
using BikeManagerV3.Customer.Data;
using BikeManagerV3.Customer.DTOs.CustomerStatistics;
using BikeManagerV3.Customer.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Customer.Services;

public class CustomerStatisticService
    : ICustomerStatisticService
{
    private readonly CustomerDbContext _context;

    public CustomerStatisticService(
        CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerStatisticResponse>
        CreateAsync(
            CreateCustomerStatisticRequest request)
    {
        var statistic = new CustomerStatistic
        {
            CustomerId = request.CustomerId,
            TotalOrders = request.TotalOrders,
            TotalSpent = request.TotalSpent,
            TotalRepairs = request.TotalRepairs,
            LastPurchaseAt = request.LastPurchaseAt,
            CustomerLevel = request.CustomerLevel,
            DiscountRate = request.DiscountRate
        };

        _context.CustomerStatistics.Add(statistic);

        await _context.SaveChangesAsync();

        return Map(statistic);
    }

    public async Task<List<CustomerStatisticResponse>>
        GetAllAsync(CustomerStatisticQuery query)
    {
        var dbQuery = _context.CustomerStatistics
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(
            query.CustomerLevel))
        {
            dbQuery = dbQuery.Where(x =>
                x.CustomerLevel ==
                query.CustomerLevel);
        }

        var statistics = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return statistics.Select(Map).ToList();
    }

    public async Task<CustomerStatisticResponse?>
        GetByIdAsync(Guid customerId)
    {
        var statistic = await _context
            .CustomerStatistics
            .FirstOrDefaultAsync(x =>
                x.CustomerId == customerId);

        if (statistic == null)
        {
            return null;
        }

        return Map(statistic);
    }

    public async Task<CustomerStatisticResponse?>
        UpdateAsync(
            Guid customerId,
            UpdateCustomerStatisticRequest request)
    {
        var statistic = await _context
            .CustomerStatistics
            .FirstOrDefaultAsync(x =>
                x.CustomerId == customerId);

        if (statistic == null)
        {
            return null;
        }

        statistic.TotalOrders = request.TotalOrders;
        statistic.TotalSpent = request.TotalSpent;
        statistic.TotalRepairs = request.TotalRepairs;
        statistic.LastPurchaseAt =
            request.LastPurchaseAt;
        statistic.CustomerLevel =
            request.CustomerLevel;
        statistic.DiscountRate =
            request.DiscountRate;

        await _context.SaveChangesAsync();

        return Map(statistic);
    }

    public async Task<bool> DeleteAsync(
        Guid customerId)
    {
        var statistic = await _context
            .CustomerStatistics
            .FirstOrDefaultAsync(x =>
                x.CustomerId == customerId);

        if (statistic == null)
        {
            return false;
        }

        _context.CustomerStatistics
            .Remove(statistic);

        await _context.SaveChangesAsync();

        return true;
    }

    private static CustomerStatisticResponse Map(
        CustomerStatistic statistic)
    {
        return new CustomerStatisticResponse
        {
            CustomerId = statistic.CustomerId,
            TotalOrders = statistic.TotalOrders,
            TotalSpent = statistic.TotalSpent,
            TotalRepairs = statistic.TotalRepairs,
            LastPurchaseAt = statistic.LastPurchaseAt,
            CustomerLevel = statistic.CustomerLevel,
            DiscountRate = statistic.DiscountRate
        };
    }
}