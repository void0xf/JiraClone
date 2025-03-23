using Microsoft.AspNetCore.Http;
using System.Net;

namespace SharedKernel;

public static class ApiResponseExtensions
{
    /// <summary>
    /// Converts a Result<T> to an ApiResponse<T>
    /// </summary>
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return ApiResponse<T>.Success(result.Value);
        }

        return ApiResponse<T>.Failure(
            result.Error.Message,
            ((int)result.Error.Code).ToString(),
            result.Error.UserMessage);
    }

    /// <summary>
    /// Converts a Result to an ApiResponse<object>
    /// </summary>
    public static ApiResponse<object> ToApiResponse(this Result result)
    {
        if (result.IsSuccess)
        {
            return ApiResponse<object>.Success(null);
        }

        return ApiResponse<object>.Failure(
            result.Error.Message,
            ((int)result.Error.Code).ToString(),
            result.Error.UserMessage);
    }

    /// <summary>
    /// Converts an ApiResponse<T> to an appropriate IResult for Minimal API
    /// </summary>
    public static IResult ToMinimalApiResult<T>(this ApiResponse<T> response)
    {
        if (response.IsSuccess)
        {
            return TypedResults.Ok(response);
        }

        // Parse error code to determine status code
        if (!int.TryParse(response.Error.Code, out var errorCodeInt))
        {
            errorCodeInt = (int)ErrorCode.UnknownError;
        }

        var errorCode = (ErrorCode)errorCodeInt;
        var errorType = DetermineErrorType(errorCode);
        var statusCode = GetStatusCode(errorType);

        var problemDetails = new HttpValidationProblemDetails
        {
            Status = (int)statusCode,
            Title = GetTitle(errorType),
            Detail = response.Error.Message,
            Type = $"https://docs.jira.com/errors/{errorCodeInt}"
        };

        // Add metadata
        problemDetails.Extensions["requestId"] = response.Error.RequestId;
        problemDetails.Extensions["timestamp"] = response.Timestamp;
        problemDetails.Extensions["errorCode"] = errorCodeInt;

        if (!string.IsNullOrEmpty(response.Error.UserMessage))
        {
            problemDetails.Extensions["userMessage"] = response.Error.UserMessage;
        }

        return TypedResults.Problem(problemDetails);
    }

    /// <summary>
    /// Converts an ApiResponse<T> to a Created result with location for Minimal API
    /// </summary>
    public static IResult ToCreatedResult<T>(this ApiResponse<T> response, string location)
    {
        if (response.IsSuccess)
        {
            return TypedResults.Created(location, response);
        }

        return response.ToMinimalApiResult();
    }

    private static ErrorType DetermineErrorType(ErrorCode code)
    {
        return code switch
        {
            ErrorCode.NotFound => ErrorType.NotFound,
            ErrorCode.ValidationFailed => ErrorType.Validation,
            ErrorCode.Unauthorized => ErrorType.Unauthorized,
            ErrorCode.Forbidden => ErrorType.Forbidden,
            ErrorCode.Conflict => ErrorType.Conflict,
            ErrorCode.UserNotFound => ErrorType.NotFound,
            ErrorCode.UserAlreadyExists => ErrorType.Conflict,
            ErrorCode.InvalidCredentials => ErrorType.Unauthorized,
            ErrorCode.ProjectsFailedToLoad => ErrorType.NotFound,
            _ => ErrorType.Unexpected
        };
    }

    /// <summary>
    /// Maps ErrorType to HTTP status code
    /// </summary>
    private static HttpStatusCode GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => HttpStatusCode.BadRequest,
        ErrorType.NotFound => HttpStatusCode.NotFound,
        ErrorType.Conflict => HttpStatusCode.Conflict,
        ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
        ErrorType.Forbidden => HttpStatusCode.Forbidden,
        _ => HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Gets a user-friendly title for an error type
    /// </summary>
    private static string GetTitle(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Validation Error",
        ErrorType.NotFound => "Resource Not Found",
        ErrorType.Conflict => "Conflict Error",
        ErrorType.Unauthorized => "Authentication Required",
        ErrorType.Forbidden => "Permission Denied",
        _ => "Server Error"
    };
} 