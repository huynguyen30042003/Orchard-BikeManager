using BikeManagerV3.Customer.Data;
using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Inventory.Enums;
using BikeManagerV3.Inventory.Models;
using BikeManagerV3.Order.Data;
using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.Enums;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Services;
using BikeManagerV3.Suppliers.Data;
using BikeManagerV3.Suppliers.DTOs.PurchaseOrder;
using BikeManagerV3.Suppliers.DTOs.PurchaseOrderItem;
using BikeManagerV3.Suppliers.Enum;
using BikeManagerV3.Suppliers.Models;
using BikeManagerV3.Suppliers.Responses;
using BikeManagerV3.Suppliers.Service.Interface;
using BikeManagerV3.Warranty.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Transactions;
namespace BikeManagerV3.Suppliers.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly SuppliersDbContext _suppliersDb;
        private readonly OrderDbContext _orderDb;
        private readonly CustomerDbContext _customerDb;
        private readonly InventoryDbContext _inventoryDb;
        private readonly CatalogDbContext _catalogDb;
        private readonly WarrantyDbContext _warrantyDb;
        private readonly ICounterService _counterService;

        public PurchaseOrderService(
            SuppliersDbContext suppliersDb,
            OrderDbContext orderDb,
            CustomerDbContext customerDb,
            InventoryDbContext inventoryDb,
            CatalogDbContext catalogDb,
            WarrantyDbContext warrantyDb,
            ICounterService counterService
            )
        {
            _orderDb = orderDb;
            _customerDb = customerDb;
            _inventoryDb = inventoryDb;
            _catalogDb = catalogDb;
            _warrantyDb = warrantyDb;
            _suppliersDb = suppliersDb;
            _counterService = counterService;
        }

        public async Task<ApiResponse<PurchaseOrderResponse>> CreateAsync(
            CreatePurchaseOrderRequest request,
            string currentUserId)
        {
            var next = await _counterService
                            .GetNextAsync(CounterCodes.PurchaseOrder);

            var PurchaseOrder =
                $"PURCHASEORDER{DateTime.UtcNow.Year}{next:D5}";
            var po = new PurchaseOrder
            {
                Id = Guid.NewGuid(),
                Code = PurchaseOrder,
                SupplierId = request.SupplierId,
                WarehouseId = request.WarehouseId,
                Status = PurchaseOrderStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserId
            };

            var variantIds = request.Items
                .Select(x => x.ProductVariantId)
                .Distinct()
                .ToList();

            var variants = await _catalogDb.ProductVariants
                .Include(x => x.Product)
                .Where(x => variantIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            foreach (var item in request.Items)
            {
                if (!variants.TryGetValue(
                        item.ProductVariantId,
                        out var productVariant))
                {
                    return ApiResponse<PurchaseOrderResponse>.Fail(
                        $"Product Variant '{item.ProductVariantId}' not found");
                }

                var total = item.Quantity * item.UnitPrice;

                po.Items.Add(new PurchaseOrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductVariantId = productVariant.Id,
                    ProductName = productVariant.Product.Name,
                    ProductVariantSKU = productVariant.SKU,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalAmount = total,
                    TrackSerial = productVariant.TrackSerial
                });

                po.SubTotal += total;
            }

            po.DiscountAmount = request.DiscountAmount;
            po.TotalAmount = po.SubTotal - po.DiscountAmount;

            await _suppliersDb.PurchaseOrders.AddAsync(po);
            await _suppliersDb.SaveChangesAsync();

            return await GetByIdAsync(po.Id);
        }
        public async Task<ApiResponse<PurchaseOrderResponse>> GetByIdAsync(Guid id)
        {
            var po = await _suppliersDb.PurchaseOrders
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (po == null)
            {
                return ApiResponse<PurchaseOrderResponse>.Fail(
                    "Purchase order not found");
            }

            return ApiResponse<PurchaseOrderResponse>.Ok(
                 new PurchaseOrderResponse
                 {
                     Id = po.Id,
                     Code = po.Code,
                     SupplierName = po.Supplier.Name,
                     TotalAmount = po.TotalAmount,
                     DiscountAmount = po.DiscountAmount,
                     Status = po.Status.ToString(),
                     CreatedAt = po.CreatedAt,

                     Items = po.Items.Select(i => new PurchaseOrderItemResponse
                     {
                         ProductVariantId = i.ProductVariantId,
                         ProductName = i.ProductName ?? string.Empty,
                         ProductVariantSKU = i.ProductVariantSKU ?? string.Empty,
                         Quantity = i.Quantity,
                         UnitPrice = i.UnitPrice,
                         TotalAmount = i.TotalAmount,
                         TrackSerial = i.TrackSerial

                     }).ToList()
                 });
        }

        public async Task<PagedResult<PurchaseOrderResponse>> GetPagedAsync(
           PurchaseOrderQuery query)
        {
            var purchaseOrders = _suppliersDb.PurchaseOrders
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                purchaseOrders = purchaseOrders.Where(x =>
                    x.Code.Contains(query.Keyword) ||
                    x.Supplier.Name.Contains(query.Keyword));
            }

            if (query.WarehouseId.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(x =>
                    x.WarehouseId == query.WarehouseId.Value);
            }

            if (query.SupplierId.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(x =>
                    x.SupplierId == query.SupplierId.Value);
            }

            if (query.Status.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(x =>
                    x.Status == query.Status.Value);
            }

            if (query.FromDate.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(x =>
                    x.CreatedAt >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                var endDate = query.ToDate.Value.Date.AddDays(1);

                purchaseOrders = purchaseOrders.Where(x =>
                    x.CreatedAt < endDate);
            }

            var total = await purchaseOrders.CountAsync();

            var poList = await purchaseOrders
                .OrderByDescending(x => x.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var items = poList.Select(x => new PurchaseOrderResponse
            {
                Id = x.Id,
                Code = x.Code,
                SupplierName = x.Supplier.Name,
                DiscountAmount = x.DiscountAmount,
                TotalAmount = x.TotalAmount,
                Status = x.Status.ToString(),
                CreatedAt = x.CreatedAt,

                Items = x.Items.Select(i => new PurchaseOrderItemResponse
                {
                    ProductVariantId = i.ProductVariantId,

                    // Snapshot data
                    ProductName = i.ProductName ?? "",
                    ProductVariantSKU = i.ProductVariantSKU ?? "",

                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalAmount = i.TotalAmount,
                    TrackSerial = i.TrackSerial

                }).ToList()
            }).ToList();

            return new PagedResult<PurchaseOrderResponse>
            {
                Items = items,
                TotalItems = total,
                Page = query.PageNumber,
                PageSize = query.PageSize,
                TotalPages = (int)Math.Ceiling(
                total / (double)query.PageSize),
            };
        }

        public async Task<ApiResponse<object>> ApproveAsync(Guid id)
        {
            var po = await _suppliersDb.PurchaseOrders
                .FirstOrDefaultAsync(x => x.Id == id);

            if (po == null)
                return ApiResponse<object>.Fail(
                "Purchase order not found");

            if (po.Status != PurchaseOrderStatus.Draft)
                return ApiResponse<object>.Fail(
                 "Only Draft can approve");
            po.Status = PurchaseOrderStatus.Approved;
            po.ApprovedAt = DateTime.UtcNow;

            await _suppliersDb.SaveChangesAsync();
            return ApiResponse<object>.Ok(
           "Approve successfully");
        }

        public async Task<ApiResponse<object>> CancelAsync(Guid id)
        {
            var po = await _suppliersDb.PurchaseOrders
                .FirstOrDefaultAsync(x => x.Id == id);

            if (po == null)
                return ApiResponse<object>.Fail("Purchase order not found");

            if (po.Status == PurchaseOrderStatus.Completed)
                return ApiResponse<object>.Fail("Purchase order has been Completed");
            if (po.Status == PurchaseOrderStatus.Cancelled)
                return ApiResponse<object>.Fail("Purchase order has been Canceled");
            po.Status = PurchaseOrderStatus.Cancelled;

            await _suppliersDb.SaveChangesAsync();
            return ApiResponse<object>.Ok(
            "Cancel successfully");
        }

        [Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
        public async Task<ApiResponse<object>> ReceiveAsync(
            Guid id,
            ReceivePurchaseOrderRequest request
            )
        {
            var po = await _suppliersDb.PurchaseOrders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (po == null)
                return ApiResponse<object>.Fail("Purchase order not found");

            if (po.Status != PurchaseOrderStatus.Approved)
                return ApiResponse<object>.Fail("Purchase order must be approved");

            foreach (var item in po.Items)
            {
                var variant = await _catalogDb.ProductVariants
                    .FirstAsync(x => x.Id == item.ProductVariantId);
                if (variant == null)
                {
                    return ApiResponse<object>.Fail(
                        "Product variant not found");
                }
                if (!variant.TrackSerial)
                {
                    variant.StockQuantity += item.Quantity;
                }
                if (variant.TrackSerial)
                {
                    if (!request.Serials.Any())
                    {
                        return ApiResponse<object>.Fail(
                        "Serials not found");
                    }
                    var serials = request.Serials
                        .Where(x => x.ProductVariantId == variant.Id)
                        .ToList();

                    if (serials.Count != item.Quantity)
                        return ApiResponse<object>.Fail($"Variant {variant.SKU} requires {item.Quantity} serials");

                    foreach (var serial in serials)
                    {
                        var next = await _counterService
                            .GetNextAsync(CounterCodes.Serial);

                        var serialCode =
                            $"SERIAL{DateTime.UtcNow.Year}{next:D5}";
                        _catalogDb.SerialNumbers.Add(new SerialNumber
                        {
                            Id = Guid.NewGuid(),
                            ProductVariantId = variant.Id,
                            SerialCode = serialCode,
                            FrameNumber = serial.FrameNumber,
                            EngineNumber = serial.EngineNumber,
                            BatterySerial = serial.BatterySerial,
                            MotorSerial = serial.MotorSerial,
                            WarehouseId = po.WarehouseId,
                            CurrentStatus = CurrentStatus.IN_STOCK,
                            ImportDate = DateTime.UtcNow
                        });
                        variant.StockQuantity++;
                    }
                }

                var stock = await _inventoryDb.InventoryStocks
                    .FirstOrDefaultAsync(x =>
                        x.ProductVariantId == item.ProductVariantId &&
                        x.WarehouseId == po.WarehouseId);
                if (stock == null)
                {
                    stock = new InventoryStock
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = po.WarehouseId,
                        ProductVariantId = item.ProductVariantId,
                        Quantity = 0,
                    };

                    _inventoryDb.InventoryStocks.Add(stock);
                }

                var beforeQty = stock.Quantity;

                stock.Quantity += item.Quantity;

                _inventoryDb.InventoryTransactions.Add(
                    new InventoryTransaction
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = po.WarehouseId,
                        ProductVariantId = item.ProductVariantId,
                        TransactionType =
                            InventoryTransactionType.Import,
                        Quantity = item.Quantity,
                        BeforeQuantity = beforeQty,
                        AfterQuantity = stock.Quantity,
                        ReferenceType = "PurchaseOrder",
                        ReferenceId = po.Id,
                        CreatedBy = po.CreatedBy,
                        CreatedAt = DateTime.UtcNow
                    });

                _inventoryDb.InventoryCostLayers.Add(
                    new InventoryCostLayer
                    {
                        Id = Guid.NewGuid(),
                        ProductVariantId = item.ProductVariantId,
                        ImportPrice = item.UnitPrice,
                        QuantityRemaining = item.Quantity,
                        ImportDate = DateTime.UtcNow
                    });
            }

            po.Status = PurchaseOrderStatus.Completed;
            po.ReceivedAt = DateTime.UtcNow;
            using var scope = new TransactionScope(
      TransactionScopeAsyncFlowOption.Enabled);

            try
            {

                await _suppliersDb.SaveChangesAsync();
                await _inventoryDb.SaveChangesAsync();
                await _catalogDb.SaveChangesAsync();

                scope.Complete();

                return ApiResponse<object>.Ok(
                    "Created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return ApiResponse<object>.Fail(
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }

        // Dùng cho sản phẩm không TrackSerial
        public async Task<ApiResponse<object>> ApplyAsync(Guid id)
        {
            await using var transaction =
                await _suppliersDb.Database.BeginTransactionAsync();

            var po = await _suppliersDb.PurchaseOrders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (po == null)
                return ApiResponse<object>.Fail(
                    "Purchase order not found");

            if (po.Status != PurchaseOrderStatus.Approved)
                return ApiResponse<object>.Fail(
                    "Purchase order must be approved");

            foreach (var item in po.Items)
            {
                // lấy variant
                var variant = await _catalogDb.ProductVariants
                    .FirstOrDefaultAsync(x =>
                        x.Id == item.ProductVariantId);

                if (variant == null)
                    return ApiResponse<object>.Fail(
                        "Product variant not found");

                // Không cho Apply nếu có serial
                if (variant.TrackSerial)
                {
                    return ApiResponse<object>.Fail(
                        $"Variant {variant.SKU} requires Receive");
                }

                // cập nhật tồn ProductVariant
                variant.StockQuantity += item.Quantity;

                // tìm stock
                var stock = await _inventoryDb.InventoryStocks
                    .FirstOrDefaultAsync(x =>
                        x.ProductVariantId == item.ProductVariantId &&
                        x.WarehouseId == po.WarehouseId);

                if (stock == null)
                {
                    stock = new InventoryStock
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = po.WarehouseId,
                        ProductVariantId = item.ProductVariantId,
                        Quantity = 0
                    };

                    _inventoryDb.InventoryStocks.Add(stock);
                }

                var beforeQty = stock.Quantity;

                stock.Quantity += item.Quantity;

                // transaction
                _inventoryDb.InventoryTransactions.Add(
                    new InventoryTransaction
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = po.WarehouseId,
                        ProductVariantId = item.ProductVariantId,
                        TransactionType =
                              InventoryTransactionType.Import,
                        Quantity = item.Quantity,
                        BeforeQuantity = beforeQty,
                        AfterQuantity = stock.Quantity,
                        ReferenceType = "PurchaseOrder",
                        ReferenceId = po.Id,
                        CreatedBy = po.CreatedBy,
                        CreatedAt = DateTime.UtcNow
                    });

                // FIFO
                _inventoryDb.InventoryCostLayers.Add(
                   new InventoryCostLayer
                   {
                       Id = Guid.NewGuid(),
                       ProductVariantId = item.ProductVariantId,
                       ImportPrice = item.UnitPrice,
                       QuantityRemaining = item.Quantity,
                       ImportDate = DateTime.UtcNow
                   });
            }

            po.Status = PurchaseOrderStatus.Completed;
            po.ReceivedAt = DateTime.UtcNow;

            using var scope = new TransactionScope(
    TransactionScopeAsyncFlowOption.Enabled);

            try
            {

                await _suppliersDb.SaveChangesAsync();
                await _inventoryDb.SaveChangesAsync();
                await _catalogDb.SaveChangesAsync();

                scope.Complete();

                return ApiResponse<object>.Ok(
                     "Created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Fail(ex.Message);
            }
        }

        private string GenerateCode()
        {
            return $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public string GenerateSerialCode()
        {
            return $"SC{DateTime.UtcNow:yyyyMMddHHmmss}";
        }
    }
}
