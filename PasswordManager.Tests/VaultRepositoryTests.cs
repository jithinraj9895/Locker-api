
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class VaultRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetVaultAsync_ReturnsCorrectVault()
    {
        // Arrange
        var context = GetDbContext();

        context.Vaults.Add(new Vault
        {
            VaultId = "123",
            EncryptedData = "My Vault is the test taddad"
        });
        await context.SaveChangesAsync();

        var repo = new VaultRepository(context);

        // Act
        var result = await repo.GetVaultAsync("123");

        // Assert
        result.Should().NotBeNull();
        result!.EncryptedData.Should().Be("My Vault is the test taddad");
    }

    [Fact]
    public async Task CreateVaultAsync_AddsVault()
    {
        // Arrange
        var context = GetDbContext();
        var repo = new VaultRepository(context);
        var vault = new Vault { VaultId = "v1", EncryptedData = "Test Vault" };

        // Act
        await repo.CreateVaultAsync(vault);

        // Assert
        context.Vaults.Count().Should().Be(1);
        context.Vaults.First().VaultId.Should().Be("v1");
    }

    [Fact]
    public async Task GetAllVaultAsync_ReturnsAllVaults()
    {
        // Arrange
        var context = GetDbContext();
        context.Vaults.Add(new Vault { VaultId = "v1", EncryptedData = "Vault 1" });
        context.Vaults.Add(new Vault { VaultId = "v2", EncryptedData = "Vault 2" });
        await context.SaveChangesAsync();
        var repo = new VaultRepository(context);

        // Act
        var all = await repo.GetAllVaultAsync();

        // Assert
        all.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateVaultAsync_UpdatesVault()
    {
        // Arrange
        var context = GetDbContext();
        var vault = new Vault { VaultId = "v1", EncryptedData = "Old Title" };
        context.Vaults.Add(vault);
        await context.SaveChangesAsync();
        var repo = new VaultRepository(context);
        vault.EncryptedData = "New Title";

        // Act
        await repo.UpdateVaultAsync(vault);
        var updated = context.Vaults.First(v => v.VaultId == "v1");

        // Assert
        updated.EncryptedData.Should().Be("New Title");
    }
}
