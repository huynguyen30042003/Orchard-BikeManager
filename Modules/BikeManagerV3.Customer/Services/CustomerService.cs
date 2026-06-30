using BikeManagerV3.Customer.Data;
using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Customer.Models;
using BikeManagerV3.Customer.Responses;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Customer.Services;

public class CustomerService : ICustomerService
{
    private readonly CustomerDbContext _context;

    public CustomerService(
        CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request)
    {
        Validate(request);

        var existedPhone = await _context.Customers
       .AnyAsync(x => x.PhoneNumber == request.PhoneNumber);

        if (existedPhone)
        {
            throw new Exception("PhoneNumber already exists");
        }
        var customer = new CustomerModel
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Gender = request.Gender,
            Birthday = request.Birthday,
            Address = request.Address,
            TotalSpent = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);

        await _context.SaveChangesAsync();

        return Map(customer);
    }

    public async Task<PagedResult<CustomerResponse>> GetAllAsync(
    CustomerQuery query)
    {
        var dbQuery = _context.Customers
            .Include(x => x.Statistic)
            .Include(x => x.Vehicles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();

            switch (query.SearchBy?.ToLower())
            {
                case "name":
                    dbQuery = dbQuery.Where(x =>
                        x.FullName.Contains(keyword));
                    break;

                case "phone":
                    dbQuery = dbQuery.Where(x =>
                        x.PhoneNumber.Contains(keyword));
                    break;

                case "email":
                    dbQuery = dbQuery.Where(x =>
                        x.Email != null &&
                        x.Email.Contains(keyword));
                    break;

                case "plate":
                    dbQuery = dbQuery.Where(x =>
                        x.Vehicles.Any(v =>
                            v.PlateNumber.Contains(keyword)));
                    break;

                case "frame":
                    dbQuery = dbQuery.Where(x =>
                        x.Vehicles.Any(v =>
                            v.FrameNumber.Contains(keyword)));
                    break;

                default:
                    dbQuery = dbQuery.Where(x =>
                        x.FullName.Contains(keyword) ||
                        x.PhoneNumber.Contains(keyword) ||
                        (x.Email != null &&
                         x.Email.Contains(keyword)) ||

                        x.Vehicles.Any(v =>
                            v.PlateNumber.Contains(keyword)) ||

                        x.Vehicles.Any(v =>
                            v.FrameNumber.Contains(keyword)));
                    break;
            }
        }

        var totalItems = await dbQuery.CountAsync();

        var customers = await dbQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<CustomerResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = customers.Select(Map).ToList()

        };
    }

    public async Task<CustomerResponse?> GetByIdAsync(
        Guid id)
    {
        var customer = await _context.Customers
            .Include(x => x.Statistic)
            .Include(x => x.Vehicles)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (customer == null)
        {
            return null;
        }

        return Map(customer);
    }

    public async Task<CustomerResponse?> GetPhoneNumberAsync(
      string PhoneNumber)
    {
        var customer = await _context.Customers
            .Include(x => x.Statistic)
            .Include(x => x.Vehicles)
            .FirstOrDefaultAsync(x => x.PhoneNumber == PhoneNumber);

        if (customer == null)
        {
            return null;
        }

        return Map(customer);
    }


    public async Task<CustomerResponse?> UpdateAsync(
        Guid id,
        UpdateCustomerRequest request)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (customer == null)
        {
            return null;
        }

        customer.FullName = request.FullName;
        customer.PhoneNumber = request.PhoneNumber;
        customer.Email = request.Email;
        customer.Gender = request.Gender;
        customer.Birthday = request.Birthday;
        customer.Address = request.Address;
        customer.TotalSpent = request.TotalSpent;

        await _context.SaveChangesAsync();

        return Map(customer);
    }

    public async Task<bool> DeleteAsync(
        Guid id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (customer == null)
        {
            return false;
        }

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();

        return true;
    }

    private static CustomerResponse Map(
        CustomerModel customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FullName = customer.FullName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Gender = customer.Gender,
            Birthday = customer.Birthday,
            Address = customer.Address,
            TotalSpent = customer.TotalSpent,
            TotalOrders = customer.Statistic?.TotalOrders ?? 0,
            LastPurchaseAt = customer.Statistic?.LastPurchaseAt,
            CustomerLevel = customer.Statistic?.CustomerLevel ?? "Normal",
            CreatedAt = customer.CreatedAt
        };
    }

    private static void Validate(
        CreateCustomerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new Exception("FullName is required");
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            throw new Exception("PhoneNumber is required");
        }
    }
}