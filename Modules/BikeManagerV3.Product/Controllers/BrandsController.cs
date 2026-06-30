using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.brand;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Product.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/brands")]
public class BrandsController : ControllerBase
{
    private readonly CatalogDbContext _context;

    public BrandsController(CatalogDbContext context)
    {
        _context = context;
    }

    // GET: api/v1/brands
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] BrandQuery query)
    {
        var brands = _context.Brands
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();

            brands = brands.Where(x =>
                x.Name.Contains(keyword) ||
                x.Slug.Contains(keyword));
        }

        var totalItems = await brands.CountAsync();

        var items = await brands
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var result = new PagedResult<Brand>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),
            Items = items
        };
        return Ok(
          ApiResponse<PagedResult<Brand>>
          .Ok(result)
      );
    }

    // GET: api/v1/brands/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var brand = await _context.Brands
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug,
                x.LogoUrl,
                x.Country,
                x.IsActive,
                x.CreatedAt
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (brand == null)
        {
            return NotFound(new
            {
                message = "Brand not found"
            });
        }

        return Ok(brand);
    }

    // GET: api/v1/brands/slug/asus
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var brand = await _context.Brands
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug,
                x.LogoUrl,
                x.Country,
                x.IsActive,
                x.CreatedAt
            })
            .FirstOrDefaultAsync(x => x.Slug == slug);

        if (brand == null)
        {
            return NotFound(new
            {
                message = "Brand not found"
            });
        }

        return Ok(brand);
    }

    // POST: api/v1/brands
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateBrandRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var slugExists = await _context.Brands
            .AnyAsync(x => x.Slug == request.Slug);

        if (slugExists)
        {
            return BadRequest(new
            {
                message = "Slug already exists"
            });
        }

        string? logoPath = null;

        // Upload logo
        if (request.Logo != null)
        {
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "brands");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(request.Logo.FileName)}";

            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await request.Logo.CopyToAsync(stream);

            logoPath = $"/uploads/brands/{fileName}";
        }

        var brand = new Brand
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            LogoUrl = logoPath,
            Country = request.Country,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Brands.Add(brand);

        await _context.SaveChangesAsync();

        return Ok(brand);
    }

    // PUT: api/v1/brands/{id}
    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateBrandRequest request)
    {
        var brand = await _context.Brands
            .FirstOrDefaultAsync(x => x.Id == id);

        if (brand == null)
        {
            return NotFound(new
            {
                message = "Brand not found"
            });
        }

        var slugExists = await _context.Brands
            .AnyAsync(x =>
                x.Slug == request.Slug &&
                x.Id != id);

        if (slugExists)
        {
            return BadRequest(new
            {
                message = "Slug already exists"
            });
        }

        // Upload new logo
        if (request.Logo != null)
        {
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "brands");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(request.Logo.FileName)}";

            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await request.Logo.CopyToAsync(stream);

            brand.LogoUrl =
                $"/uploads/brands/{fileName}";
        }

        brand.Name = request.Name;
        brand.Slug = request.Slug;
        brand.Country = request.Country;
        brand.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return Ok(brand);
    }

    // DELETE: api/v1/brands/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var brand = await _context.Brands
            .FirstOrDefaultAsync(x => x.Id == id);

        if (brand == null)
        {
            return NotFound(new
            {
                message = "Brand not found"
            });
        }

        _context.Brands.Remove(brand);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Deleted successfully"
        });
    }
}