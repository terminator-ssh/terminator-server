namespace Terminator.Core.Result;

public class Result : IResultFactory<Result>
{
    public IReadOnlyList<Error> Errors { get; }
    public ErrorType ErrorType { get; }
    public bool IsSuccessful => !Errors.Any();

    public Result(
        ErrorType errorType, 
        IReadOnlyList<Error> errors)
    {
        ErrorType = errorType;
        Errors = errors;
    }

    public static Result Success() => new(ErrorType.None, []);

    public static Result Error(ErrorType errorType, IReadOnlyList<Error> errors)
        => new(errorType, errors);

    public static Result Error(ErrorType errorType, Error error)
        => new(errorType, [error]);
}