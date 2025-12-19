public interface IVaultRepository
{
    Task<Vault?> GetVaultAsync(string vaultId);

    Task<List<Vault>> GetAllVaultAsync();
    Task CreateVaultAsync(Vault vault);
    Task UpdateVaultAsync(Vault vault);

    Task<List<VaultSummary>> GetLatestVaultsAsync(int pageNo, int limit);
}
