namespace Thoitiet.Models
{
    public class WeatherConfiguration
    {
        public Guid Id { get; set; }
        public string? PositionName { get; set; }
        public string Latitude { get; set; }
            = string.Empty;
        public string Longitude { get; set; }
            = string.Empty;
        public string Daily { get; set; }
            = string.Empty;
        public string Hourly { get; set; }
            = string.Empty;
        public string Current { get; set; }
            = string.Empty;
        public string? Timezone { get; set; }
        public DateTime CreatedAt
        { get; set; }
        public DateTime UpdatedAt
        { get; set; }
    }
}
