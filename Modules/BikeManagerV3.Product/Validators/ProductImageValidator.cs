using BikeManagerV3.Product.DTOs.ProductImage;

namespace BikeManagerV3.Product.Validators;

public static class ProductImageValidator
{
    public static List<string> ValidateCreate(
        CreateProductImageRequest request)
    {
        var errors = new List<string>();

        if (request.ProductId == Guid.Empty)
        {
            errors.Add("ProductId is required");
        }

        if (request.Image == null)
        {
            errors.Add("Image is required");
        }

        if (request.SortOrder < 0)
        {
            errors.Add("SortOrder must be >= 0");
        }

        return errors;
    }

    public static List<string> ValidateUpdate(
        UpdateProductImageRequest request)
    {
        var errors = new List<string>();

        if (request.ProductId == Guid.Empty)
        {
            errors.Add("ProductId is required");
        }

        if (request.SortOrder < 0)
        {
            errors.Add("SortOrder must be >= 0");
        }

        return errors;
    }

    public static List<string> ValidateDelete(
        Guid id)
    {
        var errors = new List<string>();

        if (id == Guid.Empty)
        {
            errors.Add("ProductId is required");
        }
        return errors;
    }
}