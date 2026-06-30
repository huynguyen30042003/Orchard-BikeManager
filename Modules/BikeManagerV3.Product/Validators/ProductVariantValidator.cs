using BikeManagerV3.Product.DTOs.ProductVariant;

namespace BikeManagerV3.Product.Validators;

public static class ProductVariantValidator
{
    public static List<string> ValidateCreate(
        CreateProductVariantRequest request)
    {
        var errors = new List<string>();

        if (request.ProductId == Guid.Empty)
        {
            errors.Add("ProductId is required");
        }

        if (request.ImportPrice < 0)
        {
            errors.Add("ImportPrice invalid");
        }

        if (request.SellingPrice < 0)
        {
            errors.Add("SellingPrice invalid");
        }

        if (request.StockQuantity < 0)
        {
            errors.Add("StockQuantity invalid");
        }

        return errors;
    }

    public static List<string> ValidateUpdate(
        UpdateProductVariantRequest request)
    {
        var errors = new List<string>();

        if (request.ProductId == Guid.Empty)
        {
            errors.Add("ProductId is required");
        }

        if (request.ImportPrice < 0)
        {
            errors.Add("ImportPrice invalid");
        }

        if (request.SellingPrice < 0)
        {
            errors.Add("SellingPrice invalid");
        }

        if (request.StockQuantity < 0)
        {
            errors.Add("StockQuantity invalid");
        }

        return errors;
    }

    public static List<string> ValidateDelete(
        Guid id)
    {
        var errors = new List<string>();

        if (id == Guid.Empty)
        {
            errors.Add("Id is required");
        }

        return errors;
    }
}