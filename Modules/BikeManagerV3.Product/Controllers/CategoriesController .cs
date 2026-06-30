using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.Category;
using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Product.Controllers;


[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CatalogDbContext _context;

    public CategoriesController(CatalogDbContext context)
    {
        _context = context;
    }

    // GET: api/categories
    // GET: api/categories?search=xe
    // GET: api/categories?parentId=xxx
    // GET: api/categories?page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CategoryQuery query)
    {
        var categories = _context.Categories
            .Include(x => x.Parent)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            categories = categories.Where(x =>
                x.Name.Contains(query.Search) ||
                x.Slug.Contains(query.Search));
        }

        // Filter parent
        if (query.ParentId.HasValue)
        {
            categories = categories.Where(x =>
                x.ParentId == query.ParentId);
        }

        var totalItems = await categories.CountAsync();

        var items = await categories
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new
            {
                x.Id,
                Parent = x.Parent == null
                    ? null
                    : new
                    {
                        x.Parent.Id,
                        x.Parent.Name,
                        x.Parent.Slug
                    },

                x.Name,
                x.Slug,
                x.ImageUrl,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt
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

    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<object>> GetById(Guid id)
    {
        var category = await _context.Categories
            .Include(x => x.Parent)
            .Select(x => new
            {
                x.Id,
                x.ParentId,

                Parent = x.Parent == null
                    ? null
                    : new
                    {
                        x.Parent.Id,
                        x.Parent.Name,
                        x.Parent.Slug
                    },

                x.Name,
                x.Slug,
                x.ImageUrl,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        {
            return ApiResponse<object>.Fail(
                "Category not found");
        }
        return ApiResponse<object>.Ok(category);
    }

    [HttpGet("tree")]
    public async Task<ApiResponse<object>> GetAllTree(
    [FromQuery] CategoryQuery query)
    {
        var categories = _context.Categories
            .AsNoTracking()
            .AsQueryable();
        var categoriesList = _context.Categories
            .Include(x => x.Parent)
            .AsQueryable();
        var totalItems = await categoriesList.CountAsync();

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            categories = categories.Where(x =>
                x.Name.Contains(query.Search) ||
                x.Children.Any(c => c.Name.Contains(query.Search)) ||
                x.Slug.Contains(query.Search));
        }

        // Nếu muốn lấy theo parent cụ thể
        if (query.ParentId.HasValue)
        {
            categories = categories.Where(x =>
                x.ParentId == query.ParentId);
        }
        else
        {
            // Chỉ lấy category gốc
            categories = categories.Where(x => x.ParentId == null);
        }

        var items = await categories
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug,
                x.ImageUrl,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt,

                Children = x.Children
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.SortOrder)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Slug,
                        c.ImageUrl,
                        c.Description,
                        c.CreatedAt,
                        c.UpdatedAt
                    })
                    .ToList()
            })
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
        return ApiResponse<object>.Ok(new
        {
            page = query.Page,
            pageSize = query.PageSize,
            totalItems,
            totalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),
            items
        });
    }

    // GET: api/categories/slug/xe-dap
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var category = await _context.Categories
            .Include(x => x.Parent)
            .Select(x => new
            {
                x.Id,
                x.ParentId,

                Parent = x.Parent == null
                    ? null
                    : new
                    {
                        x.Parent.Id,
                        x.Parent.Name,
                        x.Parent.Slug
                    },

                x.Name,
                x.Slug,
                x.ImageUrl,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt
            })
            .FirstOrDefaultAsync(x => x.Slug == slug);

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found"
            });
        }

        return Ok(category);
    }

    // POST: api/categories
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateCategoryRequest request)
    {
        var existsName = await _context.Categories
            .AnyAsync(x => x.Name == request.Name);

        if (existsName)
        {
            return BadRequest(new
            {
                message = "Category name already exists"
            });
        }

        var existsSlug = await _context.Categories
            .AnyAsync(x => x.Slug == request.Slug);

        if (existsSlug)
        {
            return BadRequest(new
            {
                message = "Category slug already exists"
            });
        }


        string? imagePath = null;

        if (request.Image != null)
        {
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "categories");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await request.Image.CopyToAsync(stream);

            imagePath =
                $"/uploads/categories/{fileName}";
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            ParentId = request.ParentId,
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            ImageUrl = imagePath
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return Ok(category);
    }

    // PUT: api/categories/{id}
    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateCategoryRequest request)
    {
        var existsName = await _context.Categories
            .AnyAsync(x => x.Name == request.Name && x.Id != id);

        if (existsName)
        {
            return BadRequest(new
            {
                message = "Name already exists"
            });
        }

        var existsSlug = await _context.Categories
            .AnyAsync(x => x.Slug == request.Slug && x.Id != id);

        if (existsName)
        {
            return BadRequest(new
            {
                message = "Slug already exists"
            });
        }
        string? imagePath = null;

        if (request.Image != null)
        {
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "categories");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await request.Image.CopyToAsync(stream);

            imagePath =
                $"/uploads/categories/{fileName}";
        }
        var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found"
            });
        }

        var slugExists = await _context.Categories
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

        category.ParentId = request.ParentId;
        category.Name = request.Name;
        category.Slug = request.Slug;
        category.ImageUrl = imagePath;
        category.Description = request.Description;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(category);
    }

    // DELETE: api/categories/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found"
            });
        }

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Deleted successfully"
        });
    }
}