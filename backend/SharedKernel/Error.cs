namespace SharedKernel;

public class Error
{
    public ErrorCode Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public Dictionary<string, object>? Metadata { get; }
    public string? UserMessage { get; }

    private Error(ErrorCode code, string message, ErrorType type, string? userMessage = null,
        Dictionary<string, object>? metadata = null)
    {
        Code = code;
        Message = message;
        Type = type;
        UserMessage = userMessage;
        Metadata = metadata;
    }
    
    public static Error NotFound(ErrorCode code, string message, string? userMessage = null)
        => new(code, message, ErrorType.NotFound, userMessage);

    public static Error Validation(ErrorCode code, string message, string? userMessage = null, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Validation, userMessage, metadata);

    public static Error Conflict(ErrorCode code, string message, string? userMessage = null)
        => new(code, message, ErrorType.Conflict, userMessage);

    public static Error Unauthorized(ErrorCode code, string message, string? userMessage = null)
        => new(code, message, ErrorType.Unauthorized, userMessage);

    public static Error Forbidden(ErrorCode code, string message, string? userMessage = null)
        => new(code, message, ErrorType.Forbidden, userMessage);

    public static Error Unexpected(ErrorCode code, string message, string? userMessage = null, Exception? exception = null)
    {
        var metadata = exception != null 
            ? new Dictionary<string, object> { { "Exception", exception.Message } } 
            : null;
                
        return new Error(code, message, ErrorType.Unexpected, userMessage, metadata);
    }
}

