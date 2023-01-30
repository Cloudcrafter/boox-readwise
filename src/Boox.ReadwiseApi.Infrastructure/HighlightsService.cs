using Boox.ReadwiseApi.Application;
using Boox.ReadwiseApi.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
namespace Boox.ReadwiseApi.Infrastructure
{
    public class HighlightsService : IHighlightsService
    {
        private readonly HttpClient _client;
        private readonly ILogger<HighlightsService> _logger;

        public HighlightsService(HttpClient client, ILogger<HighlightsService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task AddHighlightsAsync(List<ReadwiseHighlight> highlights)
        {
            try
            {
                await _client.PostAsJsonAsync("highlights", highlights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed sending highlights");
            }
        }
    }
}