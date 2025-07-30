namespace WalletAPI.Application.Shared;
public interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : Result
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}
