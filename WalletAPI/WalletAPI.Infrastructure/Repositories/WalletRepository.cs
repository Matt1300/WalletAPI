using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WalletAPI.Infrastructure.Repositories;
public class WalletRepository (ApplicationDbContext _context) : IWalletRepository
{
    public async Task AddAsync(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Wallet wallet)
    {
        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Wallet>> GetAllAsync()
    {
        return await _context.Wallets.ToListAsync();
    }

    public async Task<Wallet> GetByDocumentIdAsync(string documentId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.DocumentId == documentId);
    }

    public async Task<Wallet> GetByIdAsync(int id)
    {
        return await _context.Wallets.FindAsync(id);
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
    }
}
