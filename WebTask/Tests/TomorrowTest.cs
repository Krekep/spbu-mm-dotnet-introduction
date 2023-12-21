using Moq.Protected;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using WebTask;

namespace Tests
{
    [TestClass]
    public class TomorrowTest
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
                    ""timelines"": {
                            ""minutely"": [
                                {
                                    ""time"": ""2023-12-20T00:00:00+00:00"",
                                    ""values"": {
                                        ""temperature"": 1,
                                        ""cloudCover"": 1,
                                        ""freezingRainIntensity"": 1,
                                        ""humidity"": 1,
                                        ""rainIntensity"": 1,
                                        ""snowIntensity"": 1,
                                        ""windSpeed"": 1,
                                        ""windDirection"": 1
                                    }
                                }
                            ]
                        }
                    }"
                )
            };
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(response);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            TomorrowService stormglassService = new TomorrowService(httpClient, vars.Object);

            vars.Setup(x => x.Latitude).Returns(1);
            vars.Setup(x => x.Longitude).Returns(1);
            vars.Setup(x => x.TomorrowApiKey).Returns("a");
            vars.Setup(x => x.StormglassApiKey).Returns("a");

            var weatherData = stormglassService.GetForecast();

            Assert.IsNotNull(weatherData);
            Assert.AreEqual("1", weatherData.TemperatureC);
            Assert.AreEqual("1", weatherData.Humidity);
            Assert.AreEqual("1", weatherData.WindDirection);
            Assert.AreEqual("1", weatherData.WindSpeed);
            Assert.AreEqual("1", weatherData.CloudCover);
            Assert.AreEqual("1", weatherData.RainIntensity);
            Assert.AreEqual("1", weatherData.SnowIntensity);
            Assert.AreEqual("1", weatherData.FreezingIntensity);
        }
    }
}
