using BikeManagerV3.Customer.Data;
using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Order.Data;
using BikeManagerV3.Order.DTOs.Orders;
using BikeManagerV3.Order.Responses;
using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Services;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Order.Services;

public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly CustomerDbContext _customerDb;
    private readonly ICounterService _counterService;

    public OrderService(
        OrderDbContext context, CustomerDbContext customerDbContext, ICounterService counterService)
    {
        _context = context;
        _customerDb = customerDbContext;
        _counterService = counterService;
    }

    public async Task<ApiResponse<OrderResponse>>
        CreateAsync(CreateOrderRequest request)
    {
        var next = await _counterService
                            .GetNextAsync(CounterCodes.Order);

        var OrderCode =
            $"ORDER{DateTime.UtcNow.Year}{next:D5}";
        var order = new Models.Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            OrderCode = OrderCode,
            SubTotal = request.SubTotal,
            DiscountAmount = request.DiscountAmount,
            TaxAmount = request.TaxAmount,
            TotalAmount = request.TotalAmount,
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = request.PaymentStatus,
            OrderStatus = request.OrderStatus,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return ApiResponse<OrderResponse>.Ok(
            Map(order),
            "Created successfully");
    }

    public async Task<PagedResult<OrderResponse>>
        GetAllAsync(OrderQuery query)
    {
        var dbQuery = _context.Orders
            .AsQueryable();
        if (query.CustomerId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.CustomerId == query.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(
            query.Search))
        {
            dbQuery = dbQuery.Where(x =>
                x.OrderCode.Contains(
                    query.Search));
        }
        if (!string.IsNullOrWhiteSpace(
           query.PaymentStatus))
        {
            dbQuery = dbQuery.Where(x =>
                x.PaymentStatus == query.PaymentStatus);
        }
        if (!string.IsNullOrWhiteSpace(
          query.OrderStatus))
        {
            dbQuery = dbQuery.Where(x =>
                x.OrderStatus == query.OrderStatus);
        }

        if (query.FromDate.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.CreatedAt >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            var endDate = query.ToDate.Value.Date.AddDays(1);

            dbQuery = dbQuery.Where(x =>
                x.CreatedAt < endDate);
        }

        var orders = await dbQuery
            .OrderByDescending(x =>
                x.CreatedAt)
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
        var totalItems = await dbQuery.CountAsync();
        return new PagedResult<OrderResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = orders.Select(Map).ToList()
        };
    }

    public async Task<ApiResponse<OrderResponse>>
        GetByIdAsync(Guid id)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (order == null)
        {
            return ApiResponse<OrderResponse>
                .Fail("Order not found");
        }
        var customer = await _customerDb.Customers
            .FirstOrDefaultAsync(x => x.Id == order.CustomerId);
        var dataCustomer = new CustomerResponse
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

        var result = new OrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderCode = order.OrderCode,
            SubTotal = order.SubTotal,
            DiscountAmount =
                order.DiscountAmount,
            TaxAmount = order.TaxAmount,
            TotalAmount = order.TotalAmount,
            PaymentMethod =
                order.PaymentMethod,
            PaymentStatus =
                order.PaymentStatus,
            OrderStatus =
                order.OrderStatus,
            CreatedBy = order.CreatedBy,
            CreatedAt = order.CreatedAt,
            Customer = dataCustomer

        };
        return ApiResponse<OrderResponse>.Ok(result);

    }

    public async Task<ApiResponse<OrderResponse>>
        UpdateAsync(
            Guid id,
            UpdateOrderRequest request)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (order == null)
        {
            return ApiResponse<OrderResponse>
                .Fail("Order not found");
        }

        order.CustomerId =
            request.CustomerId;

        order.OrderCode =
            request.OrderCode;

        order.SubTotal =
            request.SubTotal;

        order.DiscountAmount =
            request.DiscountAmount;

        order.TaxAmount =
            request.TaxAmount;

        order.TotalAmount =
            request.TotalAmount;

        order.PaymentMethod =
            request.PaymentMethod;

        order.PaymentStatus =
            request.PaymentStatus;

        order.OrderStatus =
            request.OrderStatus;

        await _context.SaveChangesAsync();

        return ApiResponse<OrderResponse>.Ok(
            Map(order),
            "Updated successfully");
    }

    public async Task<ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (order == null)
        {
            return ApiResponse<string>
                .Fail("Order not found");
        }

        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static OrderResponse Map(
        Models.Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderCode = order.OrderCode,
            SubTotal = order.SubTotal,
            DiscountAmount =
                order.DiscountAmount,
            TaxAmount = order.TaxAmount,
            TotalAmount = order.TotalAmount,
            PaymentMethod =
                order.PaymentMethod,
            PaymentStatus =
                order.PaymentStatus,
            OrderStatus =
                order.OrderStatus,
            CreatedBy = order.CreatedBy,
            CreatedAt = order.CreatedAt
        };
    }
}