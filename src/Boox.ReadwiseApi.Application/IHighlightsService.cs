using Boox.ReadwiseApi.Domain.Models;

namespace Boox.ReadwiseApi.Application
{
    public interface IHighlightsService
    {
        Task AddHighlightsAsync(List<ReadwiseHighlight> highlights);
    }
}