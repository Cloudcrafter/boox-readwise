using Boox.ReadwiseApi.Application;
using Boox.ReadwiseApi.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Headers;

namespace Boox.ReadwiseApi.Tests
{
    public class HighlightsService_Tests
    {
        private IHighlightsService _service = null;
        public HighlightsService_Tests()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<HighlightsService_Tests>();

            var configuration = builder.Build();

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://readwise.io/api/v2/"),

            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", configuration["readwiseapikey"]);

            var clientFactoryMock = new Mock<IHttpClientFactory>();
            clientFactoryMock.Setup(x => x.CreateClient("readwise")).Returns(httpClient);
            _service = new HighlightsService(clientFactoryMock.Object, Mock.Of<ILogger<HighlightsService>>());
        }
        [Fact]
        public async Task AddHighlightsAsync_WhenAuthorTitleTextIsPresent_NoteAddedToReadwise()
        {
            var result = await _service.AddHighlightsAsync(new List<Domain.Models.ReadwiseHighlight>
            {
                { new Domain.Models.ReadwiseHighlight() {
                    Author = "jaib",
                    Title = "lol bog",
                    Text =  "highlight",
                    Highlighted_At = DateTime.Now.AddDays(-2),
                } }
            });

            result.Should().BeTrue();
        }
    }
}