using Microsoft.Extensions.Options;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace CondigiBack.Libs.Interfaces
{

    internal sealed class GeminiOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    // Request body

    internal sealed class GeminiRequest
    {
        public GeminiContent[] Contents { get; set; }
        public GenerationConfig GenerationConfig { get; set; }
        public SafetySettings[] SafetySettings { get; set; }
    }

    internal sealed class GeminiContent
    {
        public string Role { get; set; }
        public GeminiPart[] Parts { get; set; }
    }

    internal sealed class GeminiPart
    {
        // This one interests us the most
        public string Text { get; set; }
    }

    // Two models used for configuration
    internal sealed class GenerationConfig
    {
        public int Temperature { get; set; }
        public int TopK { get; set; }
        public int TopP { get; set; }
        public int MaxOutputTokens { get; set; }
        public List<object> StopSequences { get; set; }
    }

    internal sealed class SafetySettings
    {
        public string Category { get; set; }
        public string Threshold { get; set; }
    }

    // Response body
    internal sealed class GeminiResponse
    {
        public Candidate[] Candidates { get; set; }
        public PromptFeedback PromptFeedback { get; set; }
    }

    internal sealed class PromptFeedback
    {
        public SafetyRating[] SafetyRatings { get; set; }
    }

    internal sealed class Candidate
    {
        public Content Content { get; set; }
        public string FinishReason { get; set; }
        public int Index { get; set; }
        public SafetyRating[] SafetyRatings { get; set; }
    }

    internal sealed class Content
    {
        public Part[] Parts { get; set; }
        public string Role { get; set; }
    }

    internal sealed class Part
    {
        // This one interests us the most
        public string Text { get; set; }
    }

    internal sealed class SafetyRating
    {
        public string Category { get; set; }
        public string Probability { get; set; }
    }

    internal sealed class GeminiRequestFactory
    {
        public static GeminiRequest CreateRequest(string prompt)
        {
            return new GeminiRequest
            {
                Contents = new GeminiContent[]
                {
                new GeminiContent
                {
                    Role = "user",
                    Parts = new GeminiPart[]
                    {
                        new GeminiPart
                        {
                            Text = prompt
                        }
                    }
                }
                },
                GenerationConfig = new GenerationConfig
                {
                    Temperature = 0,
                    TopK = 1,
                    TopP = 1,
                    MaxOutputTokens = 2048,
                    StopSequences = new List<object>()
                },
                SafetySettings = new SafetySettings[]
                {
                new SafetySettings
                {
                    Category = "HARM_CATEGORY_HARASSMENT",
                    Threshold = "BLOCK_ONLY_HIGH"
                },
                new SafetySettings
                {
                    Category = "HARM_CATEGORY_HATE_SPEECH",
                    Threshold = "BLOCK_ONLY_HIGH"
                },
                new SafetySettings
                {
                    Category = "HARM_CATEGORY_SEXUALLY_EXPLICIT",
                    Threshold = "BLOCK_ONLY_HIGH"
                },
                new SafetySettings
                {
                    Category = "HARM_CATEGORY_DANGEROUS_CONTENT",
                    Threshold = "BLOCK_ONLY_HIGH"
                }
                }
            };
        }
    }

    internal sealed class GeminiDelegatingHandler(IOptions<GeminiOptions> geminiOptions)
    : DelegatingHandler
    {
        private readonly GeminiOptions _geminiOptions = geminiOptions.Value;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("x-goog-api-key", $"{_geminiOptions.ApiKey}");

            return base.SendAsync(request, cancellationToken);
        }
    }
}
