

using Microsoft.EntityFrameworkCore;

public class VaultRepository : IVaultRepository
{
    private readonly AppDbContext _context;
    public VaultRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task CreateVaultAsync(Vault vault)
    {
        vault.UpdatedAt = DateTimeOffset.UtcNow;
        _context.Vaults.Add(vault);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Vault>> GetAllVaultAsync()
    {
        return await _context.Vaults.AsNoTracking().ToListAsync();
    }

    public async Task<List<VaultSummary>> GetLatestVaultsAsync(int pageNo, int limit)
    {
        return await _context.Vaults
            .AsNoTracking()
            .OrderByDescending(v => v.UpdatedAt)
            .Skip((pageNo - 1) * limit)
            .Take(limit)
            .Select(v => new VaultSummary
            {
                vaultId = v.VaultId,
                dateTime = v.UpdatedAt
            })
            .ToListAsync();
    }


    public Task<Vault?> GetVaultAsync(string vaultId)
    {
        return _context.Vaults.AsNoTracking().FirstOrDefaultAsync(v => v.VaultId == vaultId);
    }

    public async Task UpdateVaultAsync(Vault vault)
    {
        vault.UpdatedAt = DateTimeOffset.UtcNow;
        _context.Vaults.Update(vault);
        await _context.SaveChangesAsync();
    }

}