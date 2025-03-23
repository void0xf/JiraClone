namespace SharedKernel;

public class ApiResponse<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public string RequestId { get; set; }
    public string Timestamp { get; set; }
    public ApiResponseError Error { get; set; }
    public PageInfo PageInfo { get; set; }

    private ApiResponse(bool isSuccess, T data, ApiResponseError error = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
        RequestId = Guid.NewGuid().ToString();
        Timestamp = DateTime.UtcNow.ToString("o");
    }

    public static ApiResponse<T> Success(T data)
    {
        return new ApiResponse<T>(true, data);
    }

    public static ApiResponse<T> Failure(string message, string code = null, string userMessage = null)
    {
        return new ApiResponse<T>(false, default, new ApiResponseError
        {
            Message = message,
            Code = code,
            UserMessage = userMessage,
            RequestId = Guid.NewGuid().ToString()
        });
    }
}

public class ApiResponseError
{
    public string RequestId { get; set; }
    public string Message { get; set; }
    public string Code { get; set; }
    public string UserMessage { get; set; }
}

public class PageInfo
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}