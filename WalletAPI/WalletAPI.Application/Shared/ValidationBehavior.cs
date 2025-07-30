
using FluentValidation;

namespace WalletAPI.Application.Shared;

public class ValidationBehavior<TCommand, TResponse>(
    ICommandHandler<TCommand, TResponse> _decorated,
    IEnumerable<IValidator<TCommand>> _validators) 
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TCommand command, CancellationToken cancellation)
    {
        var context = new ValidationContext<TCommand>(command);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var errors = failures.Select(f => f.ErrorMessage).ToList();
            return (TResponse)(object)Result.Failure("Validation failed.", errors);
        }

        return await _decorated.Handle(command, cancellation);
    }
}
