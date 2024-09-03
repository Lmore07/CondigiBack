using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = Environment.GetEnvironmentVariable("API_KEY_GEMINI");
    }

    public async Task<string> GenerateContentAsync(string prompt)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 1,
                topK = 64,
                topP = 0.95,
                maxOutputTokens = 8192,
                responseMimeType = "text/plain"
            }
        };

        var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}", requestContent);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GeminiResponse>(responseContent);

        return result?.candidates?[0]?.content?.parts?[0]?.text;
    }

    private class GeminiResponse
    {
        public Candidate[] candidates { get; set; }

        public class Candidate
        {
            public Content content { get; set; }
            public string finishReason { get; set; }
            public int index { get; set; }
            public safetyRatings[] safetyRatings { get; set; }
        }

        public class Content
        {
            public Part[] parts { get; set; }
            public string role { get; set; }
        }

        public class Part
        {
            public string text { get; set; }
        }

        public class safetyRatings
        {
            public string category { get; set; }
            public string probability { get; set; }
        }
    }
}
