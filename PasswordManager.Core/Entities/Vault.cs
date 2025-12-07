public class Vault
{
    public required string VaultId { get; set; }
    public required string EncryptedData { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
