using Moq;
using Moq.Protected;

using System.Net;
using System.Runtime;
using System.Web;

using WebTask;

namespace Tests
{
    [TestClass]
    public class StormglassTest
    {
        [TestMethod]
        public void TestRequest()
        {
            var vars = new Mock<IEnviromentVariables>();
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"
                    {
                        ""hours"": [
                            {
                                ""airTemperature"": {
                                    ""noaa"": 42
                                },
                                ""time"": ""2023-12-20T00:00:00+00:00"",
                                ""humidity"": {
                                    ""noaa"": 1
                                },
                                ""windDirection"": {
                                    ""noaa"": 2
                                },
                                ""windSpeed"": {
                                    ""noaa"": 3
                                },
                                ""cloudCover"": {
                                    ""noaa"": 4
                                },
                                ""precipitation"": {
                                    ""noaa"": 5
                                }
                            }
                        ]
                    }"
                )
            };
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(response);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            StormglassService stormglassService = new StormglassService(httpClient, vars.Object);

            vars.Setup(x => x.Latitude).Returns(1);
            vars.Setup(x => x.Longitude).Returns(1);
            vars.Setup(x => x.TomorrowApiKey).Returns("a");
            vars.Setup(x => x.StormglassApiKey).Returns("a");

            var weatherData = stormglassService.GetForecast();

            Assert.IsNotNull(weatherData);
            Assert.AreEqual("42", weatherData.TemperatureC);
            Assert.AreEqual("1", weatherData.Humidity);
            Assert.AreEqual("2", weatherData.WindDirection);
            Assert.AreEqual("3", weatherData.WindSpeed);
            Assert.AreEqual("4", weatherData.CloudCover);
            Assert.AreEqual("5", weatherData.RainIntensity);
            Assert.AreEqual("No data", weatherData.SnowIntensity);
            Assert.AreEqual("No data", weatherData.FreezingIntensity);
        }
    }
}