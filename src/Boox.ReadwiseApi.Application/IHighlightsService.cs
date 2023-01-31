using Boox.ReadwiseApi.Domain.Models;

namespace Boox.ReadwiseApi.Application
{
    public interface IHighlightsService
    {
        Task<bool> AddHighlightsAsync(List<ReadwiseHighlight> highlights);
    }
}