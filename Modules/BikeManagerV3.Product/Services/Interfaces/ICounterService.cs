namespace BikeManagerV3.Product.Services
{
    public interface ICounterService
    {
        Task<long> GetNextAsync(string code);
    }

}
