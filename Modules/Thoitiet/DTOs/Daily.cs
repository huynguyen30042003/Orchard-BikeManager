namespace Thoitiet.DTOs
{
    public class Daily
    {
        public string[]? time { get; set; }
        public double[]? temperature_2m_max { get; set; }
        public double[]? temperature_2m_min { get; set; }
        public double[]? weather_code { get; set; }
        public double[]? apparent_temperature_max { get; set; }
        public double[]? apparent_temperature_min { get; set; }
        public string[]? sunrise { get; set; }
        public string[]? sunset { get; set; }
        public double[]? daylight_duration { get; set; }
        public double[]? sunshine_duration { get; set; }
        public double[]? uv_index_max { get; set; }
        public double[]? uv_index_clear_sky_max { get; set; }
        public double[]? rain_sum { get; set; }
        public double[]? snowfall_sum { get; set; }
        public double[]? showers_sum { get; set; }
        public double[]? precipitation_sum { get; set; }
        public double[]? precipitation_hours { get; set; }
        public double[]? precipitation_probability_max { get; set; }
        public double[]? wind_speed_10m_max { get; set; }
        public double[]? wind_gusts_10m_max { get; set; }
        public double[]? wind_direction_10m_dominant { get; set; }
        public double[]? shortwave_radiation_sum { get; set; }
        public double[]? et0_fao_evapotranspiration { get; set; }
    }
}
