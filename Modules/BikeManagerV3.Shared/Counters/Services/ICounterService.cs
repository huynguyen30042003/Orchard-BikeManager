namespace BikeManagerV3.Shared.Counters.Services
{
    public interface ICounterService
    {
        Task<long> GetNextAsync(string code);
    }

}
