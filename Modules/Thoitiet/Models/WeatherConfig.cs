namespace Thoitiet.Models
{
    public class WeatherConfig
    {
        public Guid Id { get; set; }
        public List<string> PositionName { get; set; }
    = [];
        public List<string> Latitude { get; set; }
            = [];

        public List<string> Longitude { get; set; }
            = [];

        public List<string> Daily { get; set; }
            = [];

        public List<string> Hourly { get; set; }
            = [];

        public List<string> Current { get; set; }
            = [];

        public string? Timezone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
