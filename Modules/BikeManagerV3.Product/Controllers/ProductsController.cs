using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.Product;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Product.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly CatalogDbContext _context;
    private readonly ICounterService _counterService;

    public ProductsController(CatalogDbContext context, ICounterService counterService)
    {
        _context = context;
        _counterService = counterService;

    }

    // GET: api/v1/products
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] ProductQuery query)
    {
        var products = _context.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .AsQueryable();

        // SEARCH
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();

            products = products.Where(x =>
                x.Name.Contains(keyword) ||
                x.SKU.Contains(keyword) ||
                x.Slug.Contains(keyword));
        }

        // FILTER CATEGORY
        if (query.CategoryId.HasValue)
        {
            products = products.Where(x =>
                x.CategoryId == query.CategoryId);
        }

        // FILTER BRAND
        if (query.BrandId.HasValue)
        {
            products = products.Where(x =>
                x.BrandId == query.BrandId);
        }

        // FILTER PUBLISHED
        if (query.IsPublished.HasValue)
        {
            products = products.Where(x =>
                x.IsPublished == query.IsPublished);
        }

        var totalItems = await products.CountAsync();

        var items = await products
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new
            {
                x.Id,
                x.CategoryId,
                x.BrandId,
                x.Name,
                x.SKU,
                x.Barcode,
                x.Slug,
                x.ShortDescription,
                x.ThumbnailUrl,
                x.ProductType,
                x.IsPublished,
                x.CreatedAt,

                Category = new
                {
                    x.Category.Id,
                    x.Category.Name,
                    x.Category.Slug
                },

                Brand = new
                {
                    x.Brand.Id,
                    x.Brand.Name,
                    x.Brand.Slug,
                    x.Brand.LogoUrl
                }
            })
            .ToListAsync();

        return Ok(new
        {
            page = query.Page,
            pageSize = query.PageSize,
            totalItems,
            totalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),
            items
        });
    }

    // GET: api/v1/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.ProductVariants)
            .Include(x => x.Images)
            .Select(x => new
            {
                x.Id,
                x.CategoryId,
                x.BrandId,
                x.Name,
                x.SKU,
                x.Barcode,
                x.Slug,
                x.ShortDescription,
                x.Description,
                x.ThumbnailUrl,
                x.ProductType,
                x.IsPublished,
                x.CreatedAt,
                x.UpdatedAt,

                Category = new
                {
                    x.Category.Id,
                    x.Category.Name,
                    x.Category.Slug
                },

                Brand = new
                {
                    x.Brand.Id,
                    x.Brand.Name,
                    x.Brand.Slug,
                    x.Brand.LogoUrl
                },

                Variants = x.ProductVariants.Select(v => new
                {
                    v.Id,
                    v.SKU,
                    v.Color,
                    v.Battery,
                    v.MotorPower,
                    v.ImportPrice,
                    v.SellingPrice,
                    v.StockQuantity,
                    v.WholesalePrice,
                    v.WarrantyMonths,
                    v.TrackSerial
                }),

                Images = x.Images.Select(i => new
                {
                    i.Id,
                    i.ImageUrl,
                    i.IsThumbnail,
                    i.SortOrder
                })
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        return Ok(product);
    }

    // GET: api/v1/products/slug/macbook-pro
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var product = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Select(x => new
            {
                x.Id,
                x.CategoryId,
                x.BrandId,
                x.Name,
                x.SKU,
                x.Barcode,
                x.Slug,
                x.ShortDescription,
                x.Description,
                x.ThumbnailUrl,
                x.ProductType,
                x.IsPublished,
                x.CreatedAt,

                Category = new
                {
                    x.Category.Id,
                    x.Category.Name,
                    x.Category.Slug
                },

                Brand = new
                {
                    x.Brand.Id,
                    x.Brand.Name,
                    x.Brand.Slug,
                    x.Brand.LogoUrl
                }
            })
            .FirstOrDefaultAsync(x => x.Slug == slug);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        return Ok(product);
    }

    // POST: api/v1/products
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var categoryExists = await _context.Categories
            .AnyAsync(x => x.Id == request.CategoryId);

        if (!categoryExists)
        {
            return BadRequest(new
            {
                message = "Category does not exist"
            });
        }

        var brandExists = await _context.Brands
            .AnyAsync(x => x.Id == request.BrandId);

        if (!brandExists)
        {
            return BadRequest(new
            {
                message = "Brand does not exist"
            });
        }

        var slugExists = await _context.Products
            .AnyAsync(x => x.Slug == request.Slug);

        if (slugExists)
        {
            return BadRequest(new
            {
                message = "Slug already exists"
            });
        }

        string? thumbnailPath = null;

        // Upload thumbnail
        if (request.Thumbnail != null)
        {
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "products");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(request.Thumbnail.FileName)}";

            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await request.Thumbnail.CopyToAsync(stream);

            thumbnailPath =
                $"/uploads/products/{fileName}";
        }
        var nextBarcode = await _counterService
                            .GetNextAsync(CounterCodes.Product);

        var Barcode =
            $"PRODUCT{DateTime.UtcNow.Year}{nextBarcode:D5}";
        var nextProductSKU = await _counterService
                            .GetNextAsync(CounterCodes.ProductSKU);

        var ProductSKU =
            $"PRODUCTSKU{DateTime.UtcNow.Year}{nextProductSKU:D5}";
        var product = new ProductModels
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            Name = request.Name,
            SKU = ProductSKU,
            Barcode = Barcode,
            Slug = request.Slug,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            ThumbnailUrl = thumbnailPath,
            ProductType = request.ProductType,
            IsPublished = request.IsPublished,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // PUT: api/v1/products/{id}
    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        var categoryExists = await _context.Categories
            .AnyAsync(x => x.Id == request.CategoryId);

        if (!categoryExists)
        {
            return BadRequest(new
            {
                message = "Category does not exist"
            });
        }

        var brandExists = await _context.Brands
            .AnyAsync(x => x.Id == request.BrandId);

        if (!brandExists)
        {
            return BadRequest(new
            {
                message = "Brand does not exist"
            });
        }


        var slugExists = await _context.Products
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

        // Upload new thumbnail
        if (request.Thumbnail != null)
        {
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "products");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(request.Thumbnail.FileName)}";

            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await request.Thumbnail.CopyToAsync(stream);

            product.ThumbnailUrl =
                $"/uploads/products/{fileName}";
        }

        product.CategoryId = request.CategoryId;
        product.BrandId = request.BrandId;
        product.Name = request.Name;
        product.Barcode = request.Barcode;
        product.Slug = request.Slug;
        product.ShortDescription = request.ShortDescription;
        product.Description = request.Description;
        product.ProductType = request.ProductType;
        product.IsPublished = request.IsPublished;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // DELETE: api/v1/products/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Deleted successfully"
        });
    }
}