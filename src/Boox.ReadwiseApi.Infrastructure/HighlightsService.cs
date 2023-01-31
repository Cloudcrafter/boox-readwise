using Boox.Api.Shared.Serialization;
using Boox.ReadwiseApi.Application;
using Boox.ReadwiseApi.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Boox.ReadwiseApi.Infrastructure
{
    public class HighlightsService : IHighlightsService
    {
        private readonly HttpClient _client;
        private readonly ILogger<HighlightsService> _logger;

        public HighlightsService(IHttpClientFactory clientFactory, ILogger<HighlightsService> logger)
        {
            _client = clientFactory.CreateClient("readwise");
            _logger = logger;
        }

        public async Task<bool> AddHighlightsAsync(List<ReadwiseHighlight> highlights)
        {

            try
            {
                var response = await _client.PostCamelCaseJson("highlights/", new {highlights});
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed sending highlights");
            }

            return false;
        }
    }

    public class LOL
    {
        public string text => Guid.NewGuid().ToString();
    }

    public class Foo
    {
        public List<LOL> Highlights { get; set; } = new List<LOL>();
    }
}