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

            // Si TResponse es Result<T>
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericArg = responseType.GetGenericArguments()[0];
                var failureMethod = typeof(Result).GetMethod("Failure", 1, new[] { typeof(string), typeof(IEnumerable<string>) });
                var failureResult = failureMethod
                    .MakeGenericMethod(genericArg)
                    .Invoke(null, new object[] { "Validación Fallida.", errors });
                return (TResponse)failureResult;
            }

            // Si TResponse es Result
            if (responseType == typeof(Result))
            {
                return (TResponse)(object)Result.Failure("Validación Fallida.", errors);
            }

            throw new InvalidCastException($"No se puede crear el resultado del tipo {typeof(TResponse).Name}");
        }

        return await _decorated.Handle(command, cancellation);
    }
}