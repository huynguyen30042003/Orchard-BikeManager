using BikeManagerV3.Shared.Counters.Models;
namespace BikeManagerV3.Shared.Counters.Services
{
    public class CounterService : ICounterService
    {
        private readonly CatalogDbContext _db;

        public CounterService(CatalogDbContext db)
        {
            _db = db;
        }

        public async Task<long> GetNextAsync(string code)
        {
            var counter = await _db.Counters
                .FirstOrDefaultAsync(x => x.Code == code);

            if (counter == null)
            {
                counter = new Counter
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    CurrentValue = 1
                };

                _db.Counters.Add(counter);
            }
            else
            {
                counter.CurrentValue++;
            }

            await _db.SaveChangesAsync();

            return counter.CurrentValue;
        }
    }
}
