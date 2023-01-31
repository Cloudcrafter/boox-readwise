using System.Net.Http.Headers;
using System.Text.Json;

namespace Boox.Api.Shared.Serialization
{
    public static class HttpClientExtensions
    {
        private static MediaTypeHeaderValue JsonContentType => new MediaTypeHeaderValue("application/json");
        private static JsonSerializerOptions CamelCaseSerializerOption => new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        public static Task<HttpResponseMessage> PostCamelCaseJson<TValue>(this HttpClient client,string? uri, TValue value) 
        {
            string jsonValue = JsonSerializer.Serialize<TValue>(value, CamelCaseSerializerOption);
            return client.PostAsync(uri, new StringContent(jsonValue, JsonContentType));
        }
    }
}