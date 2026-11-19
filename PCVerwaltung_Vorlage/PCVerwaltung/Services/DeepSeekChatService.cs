using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PCVerwaltung.Services
{
    internal sealed record DeepSeekChatMessage(string Role, string Content);

    internal sealed class DeepSeekChatService
    {
        private const string Endpoint = "https://api.deepseek.com/chat/completions";
        private const string Model = "deepseek-chat";
        private static readonly HttpClient Client = new()
        {
            Timeout = TimeSpan.FromSeconds(90)
        };

        public async Task<string> SendMessageAsync(
            IReadOnlyList<DeepSeekChatMessage> conversation)
        {
            using HttpRequestMessage request = new(HttpMethod.Post, Endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                DeepSeekApiKey.GetApiKey());
            request.Content = JsonContent.Create(new
            {
                model = Model,
                messages = conversation
                    .Select(message => new
                    {
                        role = message.Role,
                        content = message.Content
                    })
                    .ToArray()
            });

            using HttpResponseMessage response = await Client.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"DeepSeek-API-Anfrage fehlgeschlagen ({(int)response.StatusCode}): {responseBody}");
            }

            using JsonDocument json = JsonDocument.Parse(responseBody);
            JsonElement answer = json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content");

            string? content = answer.GetString();
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException(
                    "DeepSeek hat eine leere Antwort zurückgegeben.");
            }

            return content;
        }
    }
}
