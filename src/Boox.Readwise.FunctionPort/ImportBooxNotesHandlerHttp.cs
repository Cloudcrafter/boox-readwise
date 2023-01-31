using Boox.NoteParser;
using Boox.ReadwiseApi.Application;
using Boox.ReadwiseApi.Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Boox.Readwise.FunctionPort
{
    public class ImportBooxNotesHandlerHttp
    {
        private readonly ILogger _logger;
        private readonly HighLightsParser _highLightsParser;
        private readonly IHighlightsService _highlightsService;

        public ImportBooxNotesHandlerHttp(ILoggerFactory loggerFactory, 
            HighLightsParser highLightsParser, IHighlightsService highlightsService)
        {
            _logger = loggerFactory.CreateLogger<ImportBooxNotesHandlerHttp>();
            _highLightsParser = highLightsParser;
            _highlightsService = highlightsService;
        }

        [Function("Function1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var fileLines = File.ReadAllLines("basb.txt");

            var highlights = _highLightsParser.GetHighlights(fileLines);

            var readwiseHighLights = new List<ReadwiseHighlight>();
            foreach(var highlight in highlights.Highlights) 
            {
                if (string.IsNullOrWhiteSpace(highlight.HighlightText))
                {
                    continue;
                }

                readwiseHighLights.Add(new ReadwiseHighlight()
                {
                    Author = highlights.Author,
                    Title = highlights.Title,
                    Highlighted_At = highlight.HighlightDate,
                    Text = highlight.HighlightText,
                    Location = highlight.PageNumber
                });
            }

           var succeeded = await _highlightsService.AddHighlightsAsync(readwiseHighLights);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
