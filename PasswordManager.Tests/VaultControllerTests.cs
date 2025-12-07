using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class VaultControllerTests
{
    private readonly Mock<ILogger<VaultController>> _loggerMock;
    private readonly Mock<IVaultRepository> _repoMock;
    private readonly VaultController _controller;

    public VaultControllerTests()
    {
        _loggerMock = new Mock<ILogger<VaultController>>();
        _repoMock = new Mock<IVaultRepository>();

        _controller = new VaultController(_loggerMock.Object, _repoMock.Object);
    }

    [Fact]
    public void Health_ReturnsOk()
    {
        // Act
        var result = _controller.Health();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("API is working 🚀", okResult.Value);
    }

    [Fact]
    public async Task GetVault_ReturnsVault_WhenFound()
    {
        // Arrange
        var vaultId = "vault1";
        var vault = new Vault { VaultId = vaultId, EncryptedData = "EncryptedData" };
        _repoMock.Setup(r => r.GetVaultAsync(vaultId)).ReturnsAsync(vault);

        // Act
        var result = await _controller.GetVault(vaultId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(vault, okResult.Value);
    }


    [Fact]
    public async Task GetAll_ReturnsListOfVaults()
    {
        // Arrange
        var vaults = new List<Vault>
        {
            new Vault { VaultId = "v1", EncryptedData = "s1" },
            new Vault { VaultId = "v2", EncryptedData = "s2" }
        };

        _repoMock.Setup(r => r.GetAllVaultAsync()).ReturnsAsync(vaults);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<List<Vault>>(result);
        Assert.Equal(2, okResult.Count);
    }
}
