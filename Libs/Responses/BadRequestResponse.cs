using CondigiBack.Libs.Interfaces;
using System.Text.Json.Serialization;

namespace CondigiBack.Libs.Responses
{
    public class BadRequestResponse<T>: GeneralResponse<T>
    {
        [JsonIgnore]
        public T Data { get; set; }

        public List<string>? Errors { get; set; }

        [JsonIgnore]
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string? Error { get; set; }
    }
}
