using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Services;
using BikeManagerV3.Suppliers.Data;
using BikeManagerV3.Suppliers.DTOs.Supplier;
using BikeManagerV3.Suppliers.Models;
using BikeManagerV3.Suppliers.Responses;
using BikeManagerV3.Suppliers.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Suppliers.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly SuppliersDbContext _db;
        private readonly ICounterService _counterService;

        public SupplierService(SuppliersDbContext db, ICounterService counterService)
        {
            _db = db;
            _counterService = counterService;
        }

        public async Task<ApiResponse<SupplierResponse>> CreateAsync(
            CreateSupplierRequest request)
        {

            var next = await _counterService
                            .GetNextAsync(CounterCodes.Supplier);

            var SupplierCode =
                $"SUPPLIER{DateTime.UtcNow.Year}{next:D5}";
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Code = SupplierCode,
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                TaxCode = request.TaxCode,
                ContactPerson = request.ContactPerson,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Suppliers.Add(supplier);

            await _db.SaveChangesAsync();

            return await GetByIdAsync(supplier.Id);
        }

        public async Task<ApiResponse<SupplierResponse>> GetByIdAsync(Guid id)
        {
            var supplier = await _db.Suppliers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (supplier == null)
                throw new Exception("Supplier not found");

            return ApiResponse<SupplierResponse>.Ok(new SupplierResponse
            {
                Id = supplier.Id,
                Code = supplier.Code,
                Name = supplier.Name,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address,
                IsActive = supplier.IsActive,
                ContactPerson = supplier.ContactPerson,
                TaxCode = supplier.TaxCode
            }
        );
        }

        public async Task DeleteAsync(Guid id)
        {
            var supplier = await _db.Suppliers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplier == null)
                throw new Exception("Supplier not found");

            supplier.IsActive = false;

            await _db.SaveChangesAsync();
        }

        public async Task<ApiResponse<SupplierResponse>> UpdateAsync(
            Guid id,
            UpdateSupplierRequest request)
        {
            var supplier = await _db.Suppliers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplier == null)
                throw new Exception("Supplier not found");

            supplier.Name = request.Name;
            supplier.Phone = request.Phone;
            supplier.Email = request.Email;
            supplier.Address = request.Address;
            supplier.TaxCode = request.TaxCode;
            supplier.ContactPerson = request.ContactPerson;

            await _db.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<PagedResult<SupplierResponse>>
            GetPagedAsync(SupplierQuery query)
        {
            var suppliers = _db.Suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                suppliers = suppliers.Where(x =>
                    x.Name.Contains(query.Keyword) ||
                    x.Code.Contains(query.Keyword));
            }

            var total = await suppliers.CountAsync();

            var items = await suppliers
                .OrderBy(x => x.Name)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new SupplierResponse
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Phone = x.Phone,
                    Email = x.Email,
                    Address = x.Address,
                    IsActive = x.IsActive,
                    TaxCode = x.TaxCode,
                    ContactPerson = x.ContactPerson
                })
                .ToListAsync();

            return new PagedResult<SupplierResponse>
            {
                Items = items,
                TotalItems = total,
                Page = query.PageNumber,
                PageSize = query.PageSize,
                TotalPages = (int)Math.Ceiling(
                total / (double)query.PageSize),
            };
        }
    }
}
