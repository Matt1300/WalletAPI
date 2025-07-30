using WalletAPI.Domain.Entities;

namespace WalletAPI.Application.Interfaces;
public interface IWalletRepository
{
    Task<Wallet> GetByIdAsync(int id, CancellationToken cancellation);
    Task<Wallet> GetByDocumentIdAsync(string documentId, CancellationToken cancellation);
    Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken cancellation);
    Task AddAsync(Wallet wallet, CancellationToken cancellationToken);
    Task UpdateAsync(Wallet wallet, CancellationToken cancellation);
    Task DeleteAsync(Wallet wallet, CancellationToken cancellation);
}
