using System.Net.Http.Headers;
using System.Text.Json;

namespace WebTask
{
    public class StormglassService : IService
    {
        private readonly HttpClient client;
        private readonly IEnviromentVariables vars;

        public StormglassService(HttpClient client, IEnviromentVariables variables)
        {
            this.client = client;
            vars = variables;
        }

        public string Name => "stormglass.io";

        public WeatherForecast GetForecast()
        {
            var currTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            var getUrlRequestString = $"https://api.stormglass.io/v2/weather/point?lat={vars.Latitude}&lng={vars.Longitude}&" +
                $"params=airTemperature,cloudCover,humidity,precipitation,windDirection,windSpeed&" +
                $"start={currTime}&end={currTime}&" +
                $"source=noaa";

            var getUrlRequest = new HttpRequestMessage(HttpMethod.Get, getUrlRequestString);
            AuthenticationHeaderValue authentication = new AuthenticationHeaderValue(vars.StormglassApiKey);
            getUrlRequest.Headers.Authorization = authentication;

            var response = client.SendAsync(getUrlRequest).Result;
            response.EnsureSuccessStatusCode();
            var content = response.Content;
            var body = content.ReadAsStringAsync().Result;

            var jsonDoc = JsonDocument.Parse(body);
            var currentForecast = jsonDoc.RootElement.GetProperty("hours").EnumerateArray().First();
            var a = currentForecast.GetProperty("airTemperature");
            var weatherForecast = new WeatherForecast
            {
                TemperatureC = currentForecast.GetProperty("airTemperature").GetProperty("noaa").GetRawText(),
                Humidity = currentForecast.GetProperty("humidity").GetProperty("noaa").GetRawText(),
                WindDirection = currentForecast.GetProperty("windDirection").GetProperty("noaa").GetRawText(),
                WindSpeed = currentForecast.GetProperty("windSpeed").GetProperty("noaa").GetRawText(),
                CloudCover = currentForecast.GetProperty("cloudCover").GetProperty("noaa").GetRawText(),
                RainIntensity = currentForecast.GetProperty("precipitation").GetProperty("noaa").GetRawText(),
                SnowIntensity = "No data",
                FreezingIntensity = "No data"
            };

            return weatherForecast;
        }
    }
}
