namespace Terminator.Core.Result;

public class Result<T> : Result, IResultFactory<Result<T>>
{
    public T? Value { get; }

    public Result(
        T? value, 
        ErrorType errorType, 
        IReadOnlyList<Error> errors) : base(errorType, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T? value) => new(value, ErrorType.None, []);

    public static Result<T> Error(ErrorType errorType, IReadOnlyList<Error> errors)
        => new(default, errorType, errors);

    public static Result<T> Error(ErrorType errorType, Error error)
        => new(default, errorType, [error]);
}