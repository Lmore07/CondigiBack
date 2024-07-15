namespace CondigiBack.Libs.Interfaces
{
    public interface GeneralResponse<T>
    {
        T Data { get; set; }
        string Message { get; set; }
        int StatusCode { get; set; }
        string? Error { get; set; }
        List<string>? Errors { get; set; }

    }
}
