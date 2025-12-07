using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class VaultController : ControllerBase
{
    private readonly ILogger<VaultController> _logger;
    private readonly IVaultRepository _vaultRepository;

    public VaultController(
        ILogger<VaultController> logger,
        IVaultRepository vaultRepository)
    {
        _logger = logger;
        _vaultRepository = vaultRepository;
    }

    [HttpGet]
    public IActionResult Health()
    {
        _logger.LogInformation("Health check endpoint called.");
        return Ok("API is working 🚀");
    }

    [HttpGet("{vaultId}")]
    public async Task<IActionResult> GetVault(string vaultId)
    {
        _logger.LogInformation("Fetching vault with ID: {vaultId}", vaultId);

        var vault = await _vaultRepository.GetVaultAsync(vaultId);

        if (vault == null)
        {
            _logger.LogWarning("Vault not found: {vaultId}", vaultId);
            return NotFound(new { message = "Vault not found" });
        }

        _logger.LogInformation("Vault {vaultId} fetched successfully", vaultId);
        return Ok(vault);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all vaults");
        var result = await _vaultRepository.GetAllVaultAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Vault vault)
    {
        _logger.LogInformation("Request received to create a new vault.");

        if (vault == null || string.IsNullOrWhiteSpace(vault.VaultId))
        {
            _logger.LogWarning("Invalid vault payload.");
            return BadRequest(new { message = "VaultId is required." });
        }

        await _vaultRepository.CreateVaultAsync(vault);

        _logger.LogInformation("Vault created with ID: {vaultId}", vault.VaultId);

        return Ok(new { success = true, vaultId = vault.VaultId });
    }
}
