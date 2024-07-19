using CondigiBack.Libs.Interfaces;
using System.Text.Json.Serialization;

namespace CondigiBack.Libs.Responses
{
    public class ErrorResponse<T> : GeneralResponse<T>
    {
        [JsonIgnore]
        public T Data { get; set; }

        [JsonIgnore]
        public List<string>? Errors { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string? Error { get; set; }

        public ErrorResponse(string message = "", string error = "", int statusCode = 0)
        {
            Message = message;
            StatusCode = statusCode;
            Error = error;
        }
    }
}
