namespace Terminator.Core.Result;

public interface IResultFactory<TResult> where TResult : IResultFactory<TResult>
{
    static abstract TResult Error(ErrorType errorType, IReadOnlyList<Error> errors);

    static abstract TResult Error(ErrorType errorType, Error error);
}