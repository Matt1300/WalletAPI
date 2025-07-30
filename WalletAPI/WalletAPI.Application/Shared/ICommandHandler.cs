namespace WalletAPI.Application.Shared;
public interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : Result
{
    /// Handles the command and returns a result.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}
