
namespace BikeManagerV3.Product.Models
{
    public class Counter
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = default!;

        public long CurrentValue { get; set; }
    }
}
