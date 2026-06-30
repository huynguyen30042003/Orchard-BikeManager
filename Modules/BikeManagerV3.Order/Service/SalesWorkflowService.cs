using Azure.Core;
using BikeManagerV3.Customer.Data;
using BikeManagerV3.Customer.Models;
using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Inventory.Enums;
using BikeManagerV3.Inventory.Models;
using BikeManagerV3.Order.Data;
using BikeManagerV3.Order.DTOs.Orders;
using BikeManagerV3.Order.DTOs.Sales;
using BikeManagerV3.Order.Models;
using BikeManagerV3.Order.Responses;
using BikeManagerV3.Order.Service.Interfaces;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.Enums;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Warranty.Data;
using BikeManagerV3.Warranty.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Transactions;
namespace BikeManagerV3.Order.Services;

public class SalesWorkflowService : ISalesWorkflowService
{
    private readonly OrderDbContext _orderDb;
    private readonly CustomerDbContext _customerDb;
    private readonly InventoryDbContext _inventoryDb;
    private readonly CatalogDbContext _catalogDb;
    private readonly WarrantyDbContext _warrantyDb;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SalesWorkflowService(
        OrderDbContext orderDb,
        CustomerDbContext customerDb,
        InventoryDbContext inventoryDb,
        CatalogDbContext catalogDb,
        WarrantyDbContext warrantyDb,
        IHttpContextAccessor httpContextAccessor)
    {
        _orderDb = orderDb;
        _customerDb = customerDb;
        _inventoryDb = inventoryDb;
        _catalogDb = catalogDb;
        _warrantyDb = warrantyDb;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<OrderResponse>> CreateSaleOrderAsync(
        CreateSaleOrderRequest request)
    {
        return await CreateSaleOrderInternalAsync(
            request,
            isInstallment: false,
            installmentRequest: null);
    }

    public async Task<ApiResponse<OrderResponse>> CreateInstallmentOrderAsync(
        CreateInstallmentOrderRequest request)
    {
        return await CreateSaleOrderInternalAsync(
            request,
            isInstallment: true,
            installmentRequest: request);
    }

    public async Task CancelOrderAsync(Guid orderId)
    {
        await ChangeOrderStatusAndReverseAsync(
            orderId,
            newStatus: "Cancelled",
            paymentStatus: "Refunded");
    }

    public async Task ReturnOrderAsync(Guid orderId)
    {
        await ChangeOrderStatusAndReverseAsync(
            orderId,
            newStatus: "Returned",
            paymentStatus: "Refunded");
    }

    private async Task<ApiResponse<OrderResponse>> CreateSaleOrderInternalAsync(
        CreateSaleOrderRequest request,
        bool isInstallment,
        CreateInstallmentOrderRequest? installmentRequest)
    {
        ValidateRequest(request);

        var now = DateTime.UtcNow;
        var customer = await GetOrCreateCustomerAsync(request.Customer);
        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?
            .FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;
        var order = new Models.Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            OrderCode = GenerateOrderCode(),
            PaymentMethod = isInstallment ? "Installment" : request.PaymentMethod,
            PaymentStatus = isInstallment ? "Partial" : "Paid",
            OrderStatus = "Completed",
            CreatedBy = userId,
            CreatedAt = now
        };

        var subTotal = 0m;
        var discountAmount = 0m;
        var taxAmount = 0m;

        foreach (var item in request.Items)
        {
            var productVariant = await _catalogDb.ProductVariants.FirstOrDefaultAsync(x => x.Id == item.ProductVariantId);

            if (productVariant == null)
            {
                return ApiResponse<OrderResponse>.Fail(
                    $"Product variant not found: {item.ProductVariantId}");
            }
            var product = await _catalogDb.Products
                .FirstAsync(x =>
                    x.ProductVariants.Any(v =>
                        v.Id == item.ProductVariantId));
            var isVehicle =
                product.ProductType is ProductType.Bicycle or ProductType.ElectricBicycle or ProductType.Motorcycle or ProductType.ElectricMotorcycle;

            if (isVehicle)
            {
                if (item.Quantity != 1)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        "Xe phải bán theo từng serial.");
                }

                if (item.SerialNumberId == null)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        "Xe phải chọn serial.");
                }
                if (item.Quantity != 1)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        "Mỗi serial chỉ được bán với Quantity = 1.");
                }
                var serial = await _catalogDb.SerialNumbers.FirstOrDefaultAsync(x => x.Id == item.SerialNumberId);

                if (serial == null)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        $"Serial not found: {item.SerialNumberId}");
                }

                if (serial.CurrentStatus != CurrentStatus.IN_STOCK)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        $"Serial {serial.SerialCode} is not available.");
                }

                if (serial.ProductVariantId != item.ProductVariantId)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        $"Serial {serial.SerialCode} không thuộc ProductVariant {item.ProductVariantId}.");
                }


                if (serial.WarehouseId == null)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        $"Serial {serial.SerialCode} không có thông tin kho.");
                }
                var warehouseId = serial.WarehouseId.Value;

                var stock = await _inventoryDb.InventoryStocks
                    .FirstOrDefaultAsync(x => x.WarehouseId == warehouseId && x.ProductVariantId == item.ProductVariantId);

                stock ??= new InventoryStock
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = warehouseId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = 0,
                    ReservedQuantity = 0,
                    UpdatedAt = now
                };
                if (stock.Quantity < 1)
                {
                    return ApiResponse<OrderResponse>.Fail(
                        $"Không đủ tồn kho cho serial {serial.SerialCode}.");
                }
                var beforeQty = stock.Quantity;
                stock.Quantity -= 1;
                stock.UpdatedAt = now;

                _inventoryDb.InventoryTransactions.Add(new InventoryTransaction
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = warehouseId,
                    ProductVariantId = item.ProductVariantId,
                    TransactionType = InventoryTransactionType.Export,
                    Quantity = 1,
                    BeforeQuantity = beforeQty,
                    AfterQuantity = stock.Quantity,
                    ReferenceType = "ORDER",
                    ReferenceId = order.Id,
                    Note = $"Bán hàng - Serial {serial.SerialCode}",
                    CreatedBy = userId,
                    CreatedAt = now
                });

                serial.CurrentStatus = CurrentStatus.SOLD;
                serial.WarrantyStart = now;
                serial.WarrantyEnd = now.AddMonths(productVariant.WarrantyMonths <= 0 ? 12 : productVariant.WarrantyMonths);

                _warrantyDb.Warranties.Add(new WarrantyModel
                {
                    Id = Guid.NewGuid(),
                    SerialNumberId = serial.Id,
                    CustomerId = customer.Id,
                    OrderId = order.Id,
                    StartDate = DateOnly.FromDateTime(now),
                    EndDate = DateOnly.FromDateTime(now.AddMonths(productVariant.WarrantyMonths <= 0 ? 12 : productVariant.WarrantyMonths)),
                    Status = "Active"
                });

                _customerDb.VehicleOwnerships.Add(new VehicleOwnership
                {
                    Id = Guid.NewGuid(),
                    SerialNumberId = serial.Id,
                    CustomerId = customer.Id,
                    OrderId = order.Id,
                    OwnershipStart = now,
                    IsCurrentOwner = true
                });
                _customerDb.CustomerVehicles.Add(new CustomerVehicle
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,

                    ModelName = productVariant.SKU,
                    FrameNumber = serial.SerialCode,
                    EngineNumber = serial.EngineNumber ?? string.Empty,
                    BatterySerial = serial.BatterySerial ?? string.Empty,

                    PlateNumber = string.Empty,
                    PurchaseDate = now
                });
            }
            productVariant.StockQuantity = Math.Max(0, (productVariant.StockQuantity ?? 0) - item.Quantity);

            var lineDiscount = Math.Max(0, item.DiscountAmount);
            var lineTotal = Math.Max(0, item.UnitPrice - lineDiscount);

            subTotal += item.UnitPrice * item.Quantity;
            discountAmount += lineDiscount;
            var revenue =
                item.UnitPrice * item.Quantity;

            var costPrice =
                await CalculateCostPriceAsync(
                    item.ProductVariantId,
                    item.Quantity);

            var profit =
                revenue - costPrice;
            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductVariantId = item.ProductVariantId,
                SerialNumberId = item.SerialNumberId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = lineDiscount,
                TotalPrice = revenue,
                CostPrice = costPrice,
                ProfitAmount = profit
            });
        }

        order.SubTotal = subTotal;
        order.DiscountAmount = discountAmount;
        order.TaxAmount = taxAmount;
        order.TotalAmount = Math.Max(0, subTotal - discountAmount + taxAmount);

        _orderDb.Orders.Add(order);
        customer.TotalSpent += order.TotalAmount;
        customer.Statistic ??= new CustomerStatistic { CustomerId = customer.Id };
        customer.Statistic.TotalOrders += 1;
        customer.Statistic.TotalSpent += order.TotalAmount;
        customer.Statistic.LastPurchaseAt = now;



        if (isInstallment && installmentRequest != null)
        {
            var provider = await _orderDb.InstallmentProviders
                .FirstOrDefaultAsync(x => x.Id == installmentRequest.ProviderId && x.IsActive);
            if (provider == null)
            {
                return ApiResponse<OrderResponse>.Fail(
                    "Installment provider not found or inactive.");
            }

            var loanAmount = installmentRequest.LoanAmount > 0
                ? installmentRequest.LoanAmount
                : Math.Max(0, order.TotalAmount - installmentRequest.DownPayment);

            var monthlyPayment = installmentRequest.InstallmentMonths <= 0
                ? 0
                : (loanAmount + (loanAmount * installmentRequest.InterestRate / 100m)) / installmentRequest.InstallmentMonths;

            _orderDb.InstallmentContracts.Add(new InstallmentContract
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProviderId = provider.Id,
                ContractNumber = GenerateContractNumber(),
                LoanAmount = loanAmount,
                DownPayment = installmentRequest.DownPayment,
                InstallmentMonths = installmentRequest.InstallmentMonths <= 0 ? 12 : installmentRequest.InstallmentMonths,
                MonthlyPayment = monthlyPayment,
                InterestRate = installmentRequest.InterestRate,
                ContractStatus = "Active"
            });
        }
        Console.WriteLine(
            _orderDb.ChangeTracker.DebugView.LongView
        );
        // Dùng shared transaction giữa tất cả DbContext
        await using var transaction = await _orderDb.Database.BeginTransactionAsync();
        try
        {
            await _catalogDb.SaveChangesAsync();
            await _inventoryDb.SaveChangesAsync();
            await _warrantyDb.SaveChangesAsync();
            await _customerDb.SaveChangesAsync();
            await _orderDb.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return ApiResponse<OrderResponse>.Ok(
            MapOrder(order),
            "Order created successfully");
    }

    private async Task ChangeOrderStatusAndReverseAsync(
        Guid orderId,
        string newStatus,
        string paymentStatus)
    {
        using var scope = new TransactionScope(
            TransactionScopeAsyncFlowOption.Enabled);
        var now = DateTime.UtcNow;

        var order = await _orderDb.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
        {
            throw new InvalidOperationException("Order not found.");
        }

        if (string.Equals(order.OrderStatus, newStatus, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var customer = await _customerDb.Customers
            .Include(x => x.Statistic)
            .FirstOrDefaultAsync(x => x.Id == order.CustomerId);

        foreach (var item in order.Items)
        {
            if (item.SerialNumberId == null)
            {
                continue;
            }

            var serial = await _catalogDb.SerialNumbers
                .FirstOrDefaultAsync(x => x.Id == item.SerialNumberId.Value);

            if (serial != null)
            {
                serial.CurrentStatus = CurrentStatus.IN_STOCK;
                serial.WarrantyStart = null;
                serial.WarrantyEnd = null;

                if (serial.WarehouseId != null)
                {
                    var warehouseId = serial.WarehouseId.Value;

                    var stock = await _inventoryDb.InventoryStocks
                        .FirstOrDefaultAsync(x =>
                            x.WarehouseId == warehouseId &&
                            x.ProductVariantId == item.ProductVariantId);

                    if (stock == null)
                    {
                        stock = new InventoryStock
                        {
                            Id = Guid.NewGuid(),
                            WarehouseId = warehouseId,
                            ProductVariantId = item.ProductVariantId,
                            Quantity = 0,
                            ReservedQuantity = 0,
                            UpdatedAt = now
                        };

                        _inventoryDb.InventoryStocks.Add(stock);
                    }

                    var beforeQty = stock.Quantity;
                    stock.Quantity += 1;
                    stock.UpdatedAt = now;

                    _inventoryDb.InventoryTransactions.Add(new InventoryTransaction
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = warehouseId,
                        ProductVariantId = item.ProductVariantId,
                        TransactionType = InventoryTransactionType.Import,
                        Quantity = 1,
                        BeforeQuantity = beforeQty,
                        AfterQuantity = stock.Quantity,
                        ReferenceType = "ORDER",
                        ReferenceId = order.Id,
                        Note = $"{newStatus} order reversal - Serial {serial.SerialCode}",
                        CreatedBy = order.CreatedBy,
                        CreatedAt = now
                    });
                }

                var productVariant = await _catalogDb.ProductVariants
                    .FirstOrDefaultAsync(x => x.Id == item.ProductVariantId);

                if (productVariant != null)
                {
                    productVariant.StockQuantity += 1;
                }
            }

            var warranty = await _warrantyDb.Warranties
                .FirstOrDefaultAsync(x =>
                    x.OrderId == order.Id &&
                    x.SerialNumberId == item.SerialNumberId.Value);

            if (warranty != null)
            {
                warranty.Status = "Cancelled";
            }

            var ownership = await _customerDb.VehicleOwnerships
                .FirstOrDefaultAsync(x =>
                    x.OrderId == order.Id &&
                    x.SerialNumberId == item.SerialNumberId.Value);

            if (ownership != null)
            {
                ownership.IsCurrentOwner = false;
                ownership.OwnershipEnd = now;
            }
        }

        if (customer != null)
        {
            customer.TotalSpent = Math.Max(0, customer.TotalSpent - order.TotalAmount);

            if (customer.Statistic != null)
            {
                customer.Statistic.TotalSpent = Math.Max(0, customer.Statistic.TotalSpent - order.TotalAmount);
                customer.Statistic.TotalOrders = Math.Max(0, customer.Statistic.TotalOrders - 1);
            }
        }

        order.OrderStatus = newStatus;
        order.PaymentStatus = paymentStatus;
        foreach (var e in _inventoryDb.ChangeTracker
             .Entries<InventoryTransaction>())
        {
            Console.WriteLine(
                $"InventoryTransaction Variant = {e.Entity.ProductVariantId}");
        }

        var dbName =
    _inventoryDb.Database.GetDbConnection().Database;

        Console.WriteLine($"Inventory DB = {dbName}");
        await _catalogDb.SaveChangesAsync();
        await _inventoryDb.SaveChangesAsync();
        await _warrantyDb.SaveChangesAsync();
        await _customerDb.SaveChangesAsync();
        await _orderDb.SaveChangesAsync();

        scope.Complete();
    }

    private async Task<CustomerModel> GetOrCreateCustomerAsync(
        CustomerInfoRequest customerInfo)
    {
        var customer = await _customerDb.Customers
            .Include(x => x.Statistic)
            .FirstOrDefaultAsync(x =>
                x.PhoneNumber == customerInfo.PhoneNumber);

        if (customer != null)
        {
            if (!string.IsNullOrWhiteSpace(customerInfo.FullName))
            {
                customer.FullName = customerInfo.FullName;
            }

            if (!string.IsNullOrWhiteSpace(customerInfo.Email))
            {
                customer.Email = customerInfo.Email;
            }

            if (!string.IsNullOrWhiteSpace(customerInfo.Address))
            {
                customer.Address = customerInfo.Address;
            }

            if (customer.Statistic == null)
            {
                customer.Statistic = new CustomerStatistic
                {
                    CustomerId = customer.Id
                };

                _customerDb.CustomerStatistics.Add(customer.Statistic);
            }

            return customer;
        }
        var existedPhone = await _customerDb.Customers
       .AnyAsync(x => x.PhoneNumber == customerInfo.PhoneNumber);

        if (existedPhone)
        {
            throw new Exception("PhoneNumber already exists");
        }
        customer = new CustomerModel
        {
            Id = Guid.NewGuid(),
            FullName = customerInfo.FullName,
            PhoneNumber = customerInfo.PhoneNumber,
            Email = customerInfo.Email,
            Address = customerInfo.Address,
            CreatedAt = DateTime.UtcNow
        };

        customer.Statistic = new CustomerStatistic
        {
            CustomerId = customer.Id
        };

        _customerDb.Customers.Add(customer);
        _customerDb.CustomerStatistics.Add(customer.Statistic);

        return customer;
    }

    private static void ValidateRequest(CreateSaleOrderRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Customer == null)
        {
            throw new InvalidOperationException("Customer is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Customer.PhoneNumber))
        {
            throw new InvalidOperationException("Customer phone number is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Customer.FullName))
        {
            throw new InvalidOperationException("Customer full name is required.");
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            throw new InvalidOperationException("At least one item is required.");
        }
    }

    private static string GenerateOrderCode()
    {
        var suffix = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        return $"SO-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{suffix}";
    }

    private static string GenerateContractNumber()
    {
        var suffix = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        return $"IC-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{suffix}";
    }

    private static OrderResponse MapOrder(Models.Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderCode = order.OrderCode,
            SubTotal = order.SubTotal,
            DiscountAmount = order.DiscountAmount,
            TaxAmount = order.TaxAmount,
            TotalAmount = order.TotalAmount,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            OrderStatus = order.OrderStatus,
            CreatedBy = order.CreatedBy,
            CreatedAt = order.CreatedAt
        };
    }

    private async Task<decimal> CalculateCostPriceAsync(
    Guid productVariantId,
    int quantity)
    {
        var layers = await _inventoryDb.InventoryCostLayers
            .Where(x =>
                x.ProductVariantId == productVariantId &&
                x.QuantityRemaining > 0)
            .OrderBy(x => x.ImportDate)
            .ToListAsync();

        var remainingQty = quantity;

        decimal totalCost = 0;

        foreach (var layer in layers)
        {
            if (remainingQty <= 0)
                break;

            var takeQty = Math.Min(
                remainingQty,
                layer.QuantityRemaining);

            totalCost +=
                takeQty * layer.ImportPrice;

            layer.QuantityRemaining -= takeQty;

            remainingQty -= takeQty;
        }

        if (remainingQty > 0)
        {
            throw new Exception(
                "Not enough inventory cost layers");
        }

        return totalCost;
    }
}