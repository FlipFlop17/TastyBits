using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Diagnostics;
using TastyBits.Model;
using TastyBits.Services;

namespace Tests
{
    [TestClass]
    public class CalorieApiServiceTest
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _client;
        private Mock<IConfiguration> _mockConfig;

        [TestMethod]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _client = new HttpClient(_mockHttpMessageHandler.Object);
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c.GetRequiredSection("CalorieNinjasApiKey").Value).Returns("YsYrL1ayu/W6jl1oTD2b2A==cf8aQhd14BbiktGN");
        }
        [TestMethod]
        public async Task GetCalorieAsync_ReturnsCorrectResult()
        {
            // Arrange
            var expected = new CalorieNinjaApiResultModel(); // Set up your expected result
            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expected))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var service = new CalorieApiService(_mockConfig.Object);

            // Act
            var result = await service.GetCalorieAsync("rice");

            // Assert
            Debug.Print("cals1: "+result.CaloriesPer100.ToString());
            Console.WriteLine("cals2: "+result.CaloriesPer100);
            Assert.AreEqual(expected, result);
        }
    }
}