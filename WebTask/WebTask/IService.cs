namespace WebTask
{
    public interface IService
    {
        string Name { get; }
        WeatherForecast GetForecast();
    }
}
