using System.Text.Json;

namespace WebTask
{
    public class TomorrowService : IService
    {
        private readonly HttpClient client;
        private readonly IEnviromentVariables vars;

        public TomorrowService(HttpClient client, IEnviromentVariables vars)
        {
            this.client = client;
            this.vars = vars;
        }

        public string Name => "tomorrow.io";

        public WeatherForecast GetForecast()
        {
            var getUrlRequest = $"https://api.tomorrow.io/v4/weather/forecast?location={vars.Latitude},{vars.Longitude}&apikey={vars.TomorrowApiKey}";

            var response = client.GetAsync(getUrlRequest).Result;
            response.EnsureSuccessStatusCode();
            var content = response.Content;
            var body = content.ReadAsStringAsync().Result;

            var jsonDoc = JsonDocument.Parse(body);
            var currentForecast = jsonDoc.RootElement.GetProperty("timelines").GetProperty("minutely").EnumerateArray().First().GetProperty("values");

            var weatherForecast = new WeatherForecast
            {
                TemperatureC = currentForecast.GetProperty("temperature").GetRawText(),
                Humidity = currentForecast.GetProperty("humidity").GetRawText(),
                WindDirection = currentForecast.GetProperty("windDirection").GetRawText(),
                WindSpeed = currentForecast.GetProperty("windSpeed").GetRawText(),
                CloudCover = currentForecast.GetProperty("cloudCover").GetRawText(),
                RainIntensity = currentForecast.GetProperty("rainIntensity").GetRawText(),
                SnowIntensity = currentForecast.GetProperty("snowIntensity").GetRawText(),
                FreezingIntensity = currentForecast.GetProperty("freezingRainIntensity").GetRawText(),
            };

            return weatherForecast;
        }
    }
}
