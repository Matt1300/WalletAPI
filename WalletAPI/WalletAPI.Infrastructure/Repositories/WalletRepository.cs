using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WalletAPI.Infrastructure.Repositories;
public class WalletRepository (ApplicationDbContext _context) : IWalletRepository
{
    public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        await _context.Wallets.AddAsync(wallet);
    }

    public async Task DeleteAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        _context.Wallets.Remove(wallet);
    }

    public async Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Wallets.ToListAsync(cancellationToken);
    }

    public async Task<Wallet> GetByDocumentIdAsync(string documentId, CancellationToken cancellationToken = default)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.DocumentId == documentId, cancellationToken);
    }

    public async Task<Wallet> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Wallets.FindAsync(id, cancellationToken);
    }

    public async Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        _context.Wallets.Update(wallet);
    }
}
