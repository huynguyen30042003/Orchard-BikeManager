
namespace BikeManagerV3.Shared.Counters.Models
{
    public class Counter
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = default!;

        public long CurrentValue { get; set; }
    }
}
