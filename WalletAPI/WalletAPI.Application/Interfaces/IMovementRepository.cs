using WalletAPI.Domain.Entities;

namespace WalletAPI.Application.Interfaces;
public interface IMovementRepository
{
    Task<Movement> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Movement>> GetByWalletIdAsync(int walletId, CancellationToken cancellationToken);
    Task<List<Movement>> GetAllTransfersAsync(CancellationToken cancellationToken);
    Task AddAsync(Movement movement, CancellationToken cancellationToken);
}
