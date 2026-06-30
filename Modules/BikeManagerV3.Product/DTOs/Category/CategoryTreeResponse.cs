
namespace BikeManagerV3.Product.DTOs.Category
{
    public class CategoryTreeResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Slug { get; set; } = default!;

        public List<CategoryTreeResponse> Children { get; set; }
            = [];
    }
}
