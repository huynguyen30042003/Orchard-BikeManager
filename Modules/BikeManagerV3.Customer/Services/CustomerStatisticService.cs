using BikeManagerV3.Customer.Data;
using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Customer.DTOs.CustomerStatistics;
using BikeManagerV3.Customer.Models;
using BikeManagerV3.Customer.Responses;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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

    public async Task<PagedResult<CustomerStatisticResponse>>
        GetAllAsync(CustomerStatisticQuery query)
    {
        const string Collation = "Vietnamese_100_CI_AI";
        var dbQuery = _context.CustomerStatistics
            .Include(x => x.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(
            query.CustomerLevel))
        {
            dbQuery = dbQuery.Where(x =>
                x.CustomerLevel ==
                query.CustomerLevel);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            dbQuery = dbQuery.Where(x =>
                EF.Functions.Collate(
                    x.Customer.FullName,
                    Collation
                ).Contains(query.Search)
                ||
                EF.Functions.Collate(
                    x.Customer.PhoneNumber,
                    Collation
                ).Contains(query.Search)
                ||
                EF.Functions.Collate(
                    x.CustomerLevel,
                    Collation
                ).Contains(query.Search)
            );
        }
        var totalItems = await dbQuery.CountAsync();

        var statistics = await dbQuery
            .OrderBy(x => x.LastPurchaseAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new CustomerStatisticResponse
            {
                CustomerId = x.CustomerId,
                TotalOrders = x.TotalOrders,
                TotalSpent = x.TotalSpent,
                TotalRepairs = x.TotalRepairs,
                LastPurchaseAt = x.LastPurchaseAt,
                CustomerLevel = x.CustomerLevel,
                DiscountRate = x.DiscountRate,
                Customer = new CustomerResponse
                {
                    Id = x.Customer.Id,
                    FullName = x.Customer.FullName,
                    PhoneNumber = x.Customer.PhoneNumber,
                    Email = x.Customer.Email,
                    Gender = x.Customer.Gender,
                    Birthday = x.Customer.Birthday,
                    Address = x.Customer.Address,
                    TotalSpent = x.Customer.TotalSpent,
                    CreatedAt = x.Customer.CreatedAt
                }
            })
            .ToListAsync();
        return new PagedResult<CustomerStatisticResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = statistics
        };
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