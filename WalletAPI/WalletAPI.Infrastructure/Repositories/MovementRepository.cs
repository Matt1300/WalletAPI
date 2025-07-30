using Microsoft.EntityFrameworkCore;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Infrastructure.Persistence;

namespace WalletAPI.Infrastructure.Repositories;
public class MovementRepository(ApplicationDbContext _context) : IMovementRepository
{
    public async Task AddAsync(Movement movement)
    {
        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();
    }

    public async Task<Movement> GetByIdAsync(int id)
    {
        return await _context.Movements.FindAsync(id);
    }

    public async Task<IEnumerable<Movement>> GetByWalletIdAsync(int walletId)
    {
        return await _context.Movements
                             .Where(m => m.WalletId == walletId)
                             .OrderByDescending(m => m.CreatedAt)
                             .ToListAsync();
    }
}
