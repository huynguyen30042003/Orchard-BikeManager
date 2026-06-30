
namespace BikeManagerV3.Product.DTOs.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int SortOrder { get; set; }

        // parent dto
        public CategoryParentDto? Parent { get; set; }
    }
}
