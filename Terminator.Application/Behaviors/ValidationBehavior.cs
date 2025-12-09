using FluentValidation;
using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResultFactory<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next.Invoke(cancellationToken);
        
        var context = new ValidationContext<TRequest>(request);
        
        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors);

        var errors = failures
            .Select(f => new Error(f.ErrorCode, f.ErrorMessage))
            .ToList();

        if (errors.Any())
            return TResponse.Error(ErrorType.Validation, errors);
        return await next.Invoke(cancellationToken);
    }
}