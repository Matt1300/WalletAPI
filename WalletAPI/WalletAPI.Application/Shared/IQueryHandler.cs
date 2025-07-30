namespace WalletAPI.Application.Shared;

public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : Result
{
    /// <summary>
    /// Handles the query and returns a result.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);

}
