using System;
using System.IO;
using Boox.NoteParser;
using Boox.NoteParser.Models;
using Boox.ReadwiseApi.Application;
using Boox.ReadwiseApi.Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Boox.Readwise.FunctionPort
{
    public class ImportBooxNotesHandlerBlob
    {
        private readonly ILogger _logger;
        private readonly HighLightsParser _highLightsParser;
        private readonly IHighlightsService _highlightsService;

        public ImportBooxNotesHandlerBlob(ILoggerFactory loggerFactory,
            HighLightsParser highLightsParser, IHighlightsService highlightsService)
        {
            _logger = loggerFactory.CreateLogger<ImportBooxNotesHandlerBlob>();
            _highLightsParser = highLightsParser;
            _highlightsService = highlightsService;
        }

        [Function("ImportBooxNotesHandlerBlob")]
        public async Task Run([BlobTrigger("notes/{name}", Connection = "blobStorage")] string incomingBlob, string name)
        {
            string[] fileLines = incomingBlob.Split('\n');

            var parsedBook  = _highLightsParser.GetHighlights(fileLines);

            var readwiseHighLights = new List<ReadwiseHighlight>();
            foreach (var highlight in parsedBook.Highlights)
            {
                if (string.IsNullOrWhiteSpace(highlight.HighlightText))
                {
                    continue;
                }

                readwiseHighLights.Add(new ReadwiseHighlight()
                {
                    Author = parsedBook.Author,
                    Title = parsedBook.Title,
                    Highlighted_At = highlight.HighlightDate,
                    Text = highlight.HighlightText,
                    Location = highlight.PageNumber,
                    Note = highlight.Note
                });
            }

            var succeeded = await _highlightsService.AddHighlightsAsync(readwiseHighLights);
        }
    }
}
