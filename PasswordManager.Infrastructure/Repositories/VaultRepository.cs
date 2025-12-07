

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
        _context.Vaults.Add(vault);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Vault>> GetAllVaultAsync()
    {
        return await _context.Vaults.ToListAsync();
    }

    public Task<Vault?> GetVaultAsync(string vaultId)
    {
        return _context.Vaults.FirstOrDefaultAsync(v => v.VaultId == vaultId);
    }

    public async Task UpdateVaultAsync(Vault vault)
    {
        _context.Vaults.Update(vault);
        await _context.SaveChangesAsync();
    }
}