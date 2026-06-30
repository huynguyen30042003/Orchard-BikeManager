using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.Brand;
using BikeManagerV3.Product.DTOs.Category;
using BikeManagerV3.Product.DTOs.ProductImage;
using BikeManagerV3.Product.DTOs.SerialNumber;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Product.DTOs;
using BikeManagerV3.Product.Responses;
using BikeManagerV3.Product.Services.Interfaces;
using BikeManagerV3.Product.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Product.Services;

public class ProductImageService
    : IProductImageService
{
    private readonly CatalogDbContext _context;
    private readonly IWebHostEnvironment _environment;
    public ProductImageService(
        CatalogDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<ApiResponse<object>> GetAll(
    Guid? productId,
    bool? isThumbnail)
    {
        // IQueryable
        var query = _context.ProductImages
            .AsQueryable();

        // FILTER
        if (productId.HasValue)
        {
            query = query.Where(x =>
                x.ProductId == productId.Value);
        }

        if (isThumbnail.HasValue)
        {
            query = query.Where(x =>
                x.IsThumbnail == isThumbnail.Value);
        }

        // SELECT DTO
        var result = await query
            .OrderBy(x => x.SortOrder)

            // Projection
            .Select(x => new ProductImageQuery
            {
                // Product Image
                Id = x.Id,
                ProductId = x.ProductId,
                ImageUrl = x.ImageUrl,
                SortOrder = x.SortOrder,
                IsThumbnail = x.IsThumbnail,

                // Product DTO
                Product = new ProductSimpleDto
                {
                    Id = x.Product.Id,
                    CategoryId = x.Product.CategoryId,
                    BrandId = x.Product.BrandId,

                    SKU = x.Product.SKU,
                    Barcode = x.Product.Barcode,
                    Slug = x.Product.Slug, 
                    Name = x.Product.Name,

                    ShortDescription = x.Product.ShortDescription,
                    Description = x.Product.Description,

                    ThumbnailUrl = x.Product.ThumbnailUrl,

                    ProductType = x.Product.ProductType,

                    // Category DTO
                    Category = new CategoryDto
                    {
                        Id = x.Product.Category.Id,
                        ParentId = x.Product.Category.ParentId,

                        Name = x.Product.Category.Name,
                        Slug = x.Product.Category.Slug,

                        ImageUrl = x.Product.Category.ImageUrl,
                        Description = x.Product.Category.Description,

                        IsActive = x.Product.Category.IsActive,
                        SortOrder = x.Product.Category.SortOrder,

                        // Parent Category DTO
                        Parent = x.Product.Category.Parent == null
                            ? null
                            : new CategoryParentDto
                            {
                                Id = x.Product.Category.Parent.Id,
                                Name = x.Product.Category.Parent.Name,
                                Slug = x.Product.Category.Parent.Slug
                            }
                    },

                    // Brand DTO
                    Brand = new BrandDto
                    {
                        Id = x.Product.Brand.Id,
                        Name = x.Product.Brand.Name,
                        LogoUrl = x.Product.Brand.LogoUrl
                    }
                }
            })

            .ToListAsync();

        return ApiResponse<object>.Ok(result);
    }

    public async Task<ApiResponse<object>> GetById(Guid id)
    {
        var data = await _context.ProductImages
            .Select(x => new ProductImageQuery
            {
                Id = x.Id,
                ProductId = x.ProductId,

                Product = x.Product == null
                    ? null
                    : new ProductSimpleDto
                    {
                        Id = x.Product.Id,
                        CategoryId = x.Product.CategoryId,
                        BrandId = x.Product.BrandId,
                        SKU = x.Product.SKU,
                        Slug = x.Product.Slug,
                        Barcode = x.Product.Barcode,
                        Name = x.Product.Name,
                        ShortDescription = x.Product.ShortDescription,
                        Description = x.Product.Description,
                        ThumbnailUrl = x.Product.ThumbnailUrl,
                        ProductType = x.Product.ProductType,

                        Category = x.Product.Category == null
                            ? null
                            : new CategoryDto
                            {
                                Id = x.Product.Category.Id,
                                ParentId = x.Product.Category.ParentId,
                                Name = x.Product.Category.Name,
                                Slug = x.Product.Category.Slug,
                                ImageUrl = x.Product.Category.ImageUrl,
                                Description = x.Product.Category.Description,
                                IsActive = x.Product.Category.IsActive,
                                SortOrder = x.Product.Category.SortOrder,

                                Parent = x.Product.Category.Parent == null
                                ? null
                                : new CategoryParentDto
                                {
                                    Id = x.Product.Category.Parent.Id,
                                    Name = x.Product.Category.Parent.Name,
                                    Slug = x.Product.Category.Parent.Slug
                                }
                            },

                        Brand = x.Product.Brand == null
                            ? null
                            : new BrandDto
                            {
                                Id = x.Product.Brand.Id,
                                Name = x.Product.Brand.Name,
                                LogoUrl = x.Product.Brand.LogoUrl
                            }
                    },

                ImageUrl = x.ImageUrl,
                SortOrder = x.SortOrder,
                IsThumbnail = x.IsThumbnail
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
        {
            return ApiResponse<object>.Fail(
                "Product image not found");
        }

        return ApiResponse<object>.Ok(data);
    }

    public async Task<ApiResponse<object>> Create(
        [FromForm] CreateProductImageRequest request)
    {
        var errors =
            ProductImageValidator.ValidateCreate(request);

        if (errors.Any())
        {
            return ApiResponse<object>.Fail(
                "Validation failed",
                errors);
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductId);

        if (product == null)
        {
            return ApiResponse<object>.Fail(
                "Product Not Found",
                errors);
        }

        if (request.Image == null || request.Image.Length == 0)
        {
            return ApiResponse<object>.Fail(
               "Image is Required",
            errors);
        }

        var uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "products");

        Directory.CreateDirectory(uploadsFolder);

        var fileName =
            $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        using (var stream = new FileStream(
                   filePath,
                   FileMode.Create))
        {
            await request.Image.CopyToAsync(stream);
        }

        var imageUrl = $"/uploads/products/{fileName}";

        // Nếu set thumbnail mới -> bỏ thumbnail cũ
        if (request.IsThumbnail)
        {
            var oldThumbnails = await _context.ProductImages
                .Where(x =>
                    x.ProductId == request.ProductId &&
                    x.IsThumbnail)
                .ToListAsync();

            foreach (var item in oldThumbnails)
            {
                item.IsThumbnail = false;
            }
        }

        var productImage = new ProductImage
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            ImageUrl = imageUrl,
            SortOrder = request.SortOrder,
            IsThumbnail = request.IsThumbnail
        };

        _context.ProductImages.Add(productImage);

        await _context.SaveChangesAsync();


        return ApiResponse<object>.Ok(
            productImage,
            "Created successfully");
    }

    public async Task<ApiResponse<object>> Update(
        Guid id,
        UpdateProductImageRequest request)
    {
        var errors =
            ProductImageValidator.ValidateUpdate(request);

        var product = await _context.Products
             .FirstOrDefaultAsync(x =>
                 x.Id == request.ProductId);

        if (product == null)
        {
            return ApiResponse<object>.Fail(
               "Product not found",
               errors);
        }

        if (request.Image == null || request.Image.Length == 0)
        {
            return ApiResponse<object>.Fail(
                "Image is required",
                errors);
        }

        var uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "products");

        Directory.CreateDirectory(uploadsFolder);

        var fileName =
            $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        using (var stream = new FileStream(
                   filePath,
                   FileMode.Create))
        {
            await request.Image.CopyToAsync(stream);
        }

        var imageUrl = $"/uploads/products/{fileName}";

        // Nếu set thumbnail mới -> bỏ thumbnail cũ
        if (request.IsThumbnail)
        {
            var oldThumbnails = await _context.ProductImages
                .Where(x =>
                    x.ProductId == request.ProductId &&
                    x.IsThumbnail)
                .ToListAsync();

            foreach (var item in oldThumbnails)
            {
                item.IsThumbnail = false;
            }
        }

        var productImage = new ProductImage
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            ImageUrl = imageUrl,
            SortOrder = request.SortOrder,
            IsThumbnail = request.IsThumbnail
        };

        _context.ProductImages.Add(productImage);

        await _context.SaveChangesAsync();



        return ApiResponse<object>.Ok(
            productImage,
            "Updated successfully");
    }

    public async Task<ApiResponse<object>> Delete(Guid id)
    {
        var productImage = await _context.ProductImages
            .FirstOrDefaultAsync(x => x.Id == id);
        var errors =
            ProductImageValidator.ValidateDelete(id);
        if (productImage == null)
        {
            return ApiResponse<object>.Fail(
                "Product image not found",
                errors);
        }

        // Xóa file vật lý
        if (!string.IsNullOrEmpty(productImage.ImageUrl))
        {
            var relativePath = productImage.ImageUrl
                .TrimStart('/')
                .Replace("/", Path.DirectorySeparatorChar.ToString());

            var fullPath = Path.Combine(
                _environment.WebRootPath,
                relativePath);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        _context.ProductImages.Remove(productImage);

        await _context.SaveChangesAsync();

        return ApiResponse<object>.Ok(
            null,
            "Deleted successfully");
    }

    public Task GetAll(SerialNumberQuery query)
    {
        throw new NotImplementedException();
    }
}