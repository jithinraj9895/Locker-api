public interface IVaultRepository
{
    Task<Vault?> GetVaultAsync(string vaultId);

    Task<List<Vault>> GetAllVaultAsync();
    Task CreateVaultAsync(Vault vault);
    Task UpdateVaultAsync(Vault vault);
}
