using WalletAPI.Application.Interfaces;

namespace WalletAPI.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IWalletRepository Wallets { get; }
    IMovementRepository Movements { get; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
