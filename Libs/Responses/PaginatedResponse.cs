using CondigiBack.Libs.Interfaces;
using System.Text.Json.Serialization;

namespace CondigiBack.Libs.Responses
{
    public class PaginatedResponse<T> : GeneralResponse<IEnumerable<T>>
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<T> Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        [JsonIgnore]
        public string? Error { get; set; }

        public PaginatedResponse(IEnumerable<T> data, int statusCode, string message = "", Pagination pagination = null)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
            Pagination = pagination;
        }
    }
}
