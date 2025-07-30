using WalletAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using WalletAPI.Application.Interfaces;
using WalletAPI.Infrastructure.Persistence;

namespace WalletAPI.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _transaction;

    public IWalletRepository Wallets { get; }
    public IMovementRepository Movements { get; }

    public UnitOfWork(ApplicationDbContext context, IWalletRepository wallets, IMovementRepository movements)
    {
        _context = context;
        Wallets = wallets;
        Movements = movements;
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction == null)
            _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
