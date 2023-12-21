namespace WebTask
{
    public class WeatherForecast
    {
        public string? CloudCover { get; set; }
        public string? Humidity { get; set; }
        public string? RainIntensity { get; set; }
        public string? FreezingIntensity { get; set; }
        public string? SnowIntensity { get; set; }
        public string? WindDirection { get; set; }
        public string? WindSpeed { get; set; }
        public string TemperatureC { get; set; }
        public string TemperatureF => (32 + (int)(double.Parse(TemperatureC) / 0.5556)).ToString();
    }
}
