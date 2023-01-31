using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Boox.Readwise.FunctionPort
{
    public class ImportBooxNotesHandlerBlob
    {
        private readonly ILogger _logger;

        public ImportBooxNotesHandlerBlob(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ImportBooxNotesHandlerBlob>();
        }

        [Function("ImportBooxNotesHandlerBlob")]
        public void Run([BlobTrigger("books/{name}", Connection = "blobStorage")] string myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
        }
    }
}
