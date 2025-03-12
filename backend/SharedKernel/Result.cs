using System.Net;
using Microsoft.AspNetCore.Http;
namespace SharedKernel;
/// <summary>
/// Represents the outcome of an operation, encapsulating success or failure.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// The error associated with a failed operation, if any.
    /// </summary>
    public Error? Error { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// Ensures that a success result does not contain an error and vice versa.
    /// </summary>
    /// <param name="isSuccess">Indicates if the operation was successful.</param>
    /// <param name="error">The error associated with the result (if any).</param>
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("Cannot create a success result with an error.");

        if (!isSuccess && error == null)
            throw new InvalidOperationException("Cannot create a failure result without an error.");
        
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result Success() => new Result(true, null);

    /// <summary>
    /// Creates a failed result with an associated error.
    /// </summary>
    /// <param name="error">The error causing the failure.</param>
    public static Result Failure(Error error) => new Result(false, error);
}

/// <summary>
/// Represents the outcome of an operation that returns a value.
/// </summary>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public class Result<T> : Result
{
    private readonly T _value;

    /// <summary>
    /// Gets the value associated with a successful operation.
    /// Throws an exception if accessed on a failed result.
    /// </summary>
    public T Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access Value of a failed result");

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    protected Result(bool isSuccess, Error error, T value) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Creates a successful result containing a value.
    /// </summary>
    /// <param name="data">The value returned on success.</param>
    public static Result<T> Success(T data) => new Result<T>(true, null, data);

    /// <summary>
    /// Creates a failed result with an associated error.
    /// </summary>
    /// <param name="error">The error causing the failure.</param>
    public static Result<T> Failure(Error error) => new Result<T>(false, error, default);
    
    /// <summary>
    /// Implicitly converts a value to a successful result.
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);
}


    /// <summary>
    /// Extensions to convert Result objects to Minimal API HTTP results
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Converts a Result to an appropriate IResult for Minimal API
        /// </summary>
        public static IResult ToMinimalApiResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return TypedResults.Ok();
            }

            return CreateProblemResult(result.Error!);
        }

        /// <summary>
        /// Converts a Result<T> to an appropriate IResult for Minimal API
        /// </summary>
        public static IResult ToMinimalApiResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result.Value);
            }

            return CreateProblemResult(result.Error!);
        }

        /// <summary>
        /// Creates a ProblemDetails result from an Error
        /// </summary>
        private static IResult CreateProblemResult(Error error)
        {
            var statusCode = GetStatusCode(error.Type);
            var problemDetails = new HttpValidationProblemDetails
            {
                Status = (int)statusCode,
                Title = GetTitle(error.Type),
                Detail = error.Message,
                Type = $"https://docs.jira.com/errors/{(int)error.Code}"
            };

            // Add error code to the extensions
            problemDetails.Extensions["errorCode"] = (int)error.Code;
            
            // Add user-friendly message if available
            if (!string.IsNullOrEmpty(error.UserMessage))
            {
                problemDetails.Extensions["userMessage"] = error.UserMessage;
            }

            // Add metadata if available
            if (error.Metadata != null)
            {
                foreach (var (key, value) in error.Metadata)
                {
                    problemDetails.Extensions[key] = value;
                }
            }

            return TypedResults.Problem(problemDetails);
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