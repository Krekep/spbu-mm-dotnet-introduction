using Microsoft.AspNetCore.Mvc;

namespace WebTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private Dictionary<string, IService> services = new Dictionary<string, IService>();

        public WeatherForecastController(IEnumerable<IService> weatherServices)
        {
            services = new Dictionary<string, IService>();
            foreach (var service in weatherServices)
            {
                services[service.Name] = service;
            }
        }
        [HttpGet("ServicesName")]
        public IActionResult GetServices() => Ok(services.Keys.ToList());

        [HttpGet("AllForecasts")]
        public IEnumerable<WeatherForecast> GetFromAll()
        {
            List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();
            foreach (var service in services)
            {
                weatherForecasts.Add(service.Value.GetForecast());
            }
            return weatherForecasts;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public WeatherForecast GetFromSingle([FromQuery] string serviceName)
        {
            return services[serviceName].GetForecast();
        }
    }
}
