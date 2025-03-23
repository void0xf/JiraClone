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