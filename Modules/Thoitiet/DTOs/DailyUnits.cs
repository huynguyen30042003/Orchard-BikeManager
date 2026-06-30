namespace Thoitiet.DTOs
{
    public class DailyUnits
    {
        public string? time { get; set; }
        public string? temperature_2m_max { get; set; }
        public string? temperature_2m_min { get; set; }
        public string? weather_code { get; set; }
        public string? apparent_temperature_max { get; set; }
        public string? apparent_temperature_min { get; set; }
        public string? sunrise { get; set; }
        public string? sunset { get; set; }
        public string? daylight_duration { get; set; }
        public string? sunshine_duration { get; set; }
        public string? uv_index_max { get; set; }
        public string? uv_index_clear_sky_max { get; set; }
        public string? rain_sum { get; set; }
        public string? snowfall_sum { get; set; }
        public string? showers_sum { get; set; }
        public string? precipitation_sum { get; set; }
        public string? precipitation_hours { get; set; }
        public string? precipitation_probability_max { get; set; }
        public string? wind_speed_10m_max { get; set; }
        public string? wind_gusts_10m_max { get; set; }
        public string? wind_direction_10m_dominant { get; set; }
        public string? shortwave_radiation_sum { get; set; }
        public string? et0_fao_evapotranspiration { get; set; }
    }
}

