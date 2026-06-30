using BikeManagerV3.Product.DTOs.SerialNumber;

namespace BikeManagerV3.Product.Validators;

public static class SerialNumberValidator
{
    public static List<string> ValidateCreate(
        CreateSerialNumberRequest request)
    {
        var errors = new List<string>();

        if (request.ProductVariantId == Guid.Empty)
        {
            errors.Add("ProductVariantId is required");
        }
        return errors;
    }

    public static List<string> ValidateUpdate(
        UpdateSerialNumberRequest request)
    {
        var errors = new List<string>();

        if (request.ProductVariantId == Guid.Empty)
        {
            errors.Add("ProductVariantId is required");
        }

        if (string.IsNullOrWhiteSpace(
                request.SerialCode))
        {
            errors.Add("SerialCode is required");
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