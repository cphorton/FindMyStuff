using FluentValidation;
using MediatR.Pipeline;

namespace FindMyStuff.Api.Shared;

public class ValidationBehaviour<TRequest>(IEnumerable<IValidator<TRequest>> validators) 
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return;

        var vTasks = _validators.Select(x => x.ValidateAsync(request, cancellationToken)).ToArray();
        await Task.WhenAll(vTasks);

        var errors = vTasks.Select(x => x.Result)
            .SelectMany(x => x.Errors)
            .Where(x => x != null);

        if (errors.Any())
        {
            throw new ValidationException("Validation Failed.", errors, typeof(TRequest));
        }
    }
}
