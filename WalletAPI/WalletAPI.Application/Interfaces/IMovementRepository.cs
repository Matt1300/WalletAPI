using WalletAPI.Domain.Entities;

namespace WalletAPI.Application.Interfaces;
public interface IMovementRepository
{
    Task<Movement> GetByIdAsync(int id);
    Task<IEnumerable<Movement>> GetByWalletIdAsync(int walletId);
    Task AddAsync(Movement movement);
}
