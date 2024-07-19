using CondigiBack.Libs.Interfaces;
using System.Text.Json.Serialization;

namespace CondigiBack.Libs.Responses
{
    public class PaginatedResponse<T> : GeneralResponse<T>
    {
        public Pagination Pagination { get; set; }

        [JsonIgnore]
        public List<string>? Errors { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        [JsonIgnore]
        public string? Error { get; set; }

        public PaginatedResponse(T data, int statusCode, string message = "", Pagination pagination = null)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
            Pagination = pagination;
        }
    }
}
