using Boox.NoteParser;
using Boox.ReadwiseApi.Application;
using Boox.ReadwiseApi.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddScoped<IHighlightsService, HighlightsService>();
        services.AddSingleton<HighLightsParser>();

        services.AddHttpClient("readwise", client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("ReadWiseHighlightsApiUrl"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", configuration.GetValue<string>("ReadWiseApiKey"));
        });
    })
    .Build();

host.Run();
