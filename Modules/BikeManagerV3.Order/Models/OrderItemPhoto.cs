using BikeManagerV3.Order.Enum;

namespace BikeManagerV3.Order.Models
{
    public class OrderItemPhoto
    {
        public Guid Id { get; set; }

        public Guid OrderItemId { get; set; }

        public OrderPhotoType PhotoType { get; set; } = OrderPhotoType.FullBike;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public OrderItem OrderItem { get; set; } = null!;
    }
}
