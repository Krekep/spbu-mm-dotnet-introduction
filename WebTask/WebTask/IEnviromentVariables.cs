namespace WebTask
{
    public interface IEnviromentVariables
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public string StormglassApiKey { get; }
        public string TomorrowApiKey { get; }
    }
}
