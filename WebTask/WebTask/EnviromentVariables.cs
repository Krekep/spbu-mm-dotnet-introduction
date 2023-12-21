namespace WebTask
{
    // for mock testing, otherwise I prefer static class
    // I could make these settings customizable, but I don't like it
    public class EnviromentVariables : IEnviromentVariables
    {

        public double Latitude => double.Parse(GetValueFromSystem("Latitude"));
        public double Longitude => double.Parse(GetValueFromSystem("Longitude"));
        public string StormglassApiKey => GetValueFromSystem("StormglassApiKey");
        public string TomorrowApiKey => GetValueFromSystem("TomorrowApiKey");

        private static string GetValueFromSystem(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (value == null)
                throw new Exception($"Variable {name} was not found!");
            return value;
        }
    }
}
