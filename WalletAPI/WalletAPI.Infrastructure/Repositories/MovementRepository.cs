using Microsoft.EntityFrameworkCore;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Infrastructure.Persistence;

namespace WalletAPI.Infrastructure.Repositories;
public class MovementRepository(ApplicationDbContext _context) : IMovementRepository
{
    public async Task AddAsync(Movement movement, CancellationToken cancellationToken)
    {
        await _context.Movements.AddAsync(movement, cancellationToken);
    }

    public Task<List<Movement>> GetAllTransfersAsync(CancellationToken cancellationToken)
    {
        return _context.Movements
            .AsNoTracking()
            .Include(m => m.Wallet)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Movement> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Movements.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Movement>> GetByWalletIdAsync(int walletId, CancellationToken cancellationToken)
    {
        return await _context.Movements
                             .Where(m => m.WalletId == walletId)
                             .OrderByDescending(m => m.CreatedAt)
                             .ToListAsync(cancellationToken);
    }
}
