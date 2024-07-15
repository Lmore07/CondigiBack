using CondigiBack.Libs.Interfaces;
using System.Text.Json.Serialization;

namespace CondigiBack.Libs.Responses
{
    public class StandardResponse<T> : GeneralResponse<T>
    {
        [JsonIgnore]
        public List<string>? Errors { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        [JsonIgnore]
        public string? Error { get; set; }

        public StandardResponse(T data, string message = "", int statusCode = 0)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }
    }
}
