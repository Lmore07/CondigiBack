using System.Text;
using System.Text.Json;

namespace CondigiBack.Libs.Services
{
    public class EmailService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("API_KEY_BREVO");
        }

        public async Task SendEmailAsync(string from, string nameFrom, string to, string toName, string pdfUrl)
        {
            var body = new
            {
                templateId = 1,
                to = new[] { new { email = to, name = toName } },
                cc = new[] { new { email = from, name = nameFrom } },
                attachment = new[] { new { name = "Nuevo_Contrato.pdf", url = pdfUrl } }
            };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
            {
                Content = content
            };
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("api-key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
        }
    }
}
