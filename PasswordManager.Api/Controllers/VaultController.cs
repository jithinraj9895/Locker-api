using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class VaultController : ControllerBase
{
    
    private readonly IVaultRepository _vaultRepository;
    private readonly ILogger<VaultController> _logger;

    public VaultController(IVaultRepository vaultRepository, ILogger<VaultController> logger)
    {
        _vaultRepository = vaultRepository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get() => Ok("Works fine!");


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Fetching all the data from Database.");
            var allVaults = await _vaultRepository.GetAllVaultAsync();
            return Ok(allVaults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all vaults");
            return StatusCode(500, "An error occurred while fetching vaults.");
        }
    }

    [HttpGet("{vaultId}")]
    public async Task<IActionResult> GetVault(string vaultId)
    {
        try
        {
            var vault = await _vaultRepository.GetVaultAsync(vaultId);
            if (vault == null) return NotFound();
            return Ok(vault);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching vault {vaultId}");
            return StatusCode(500, "An error occurred while fetching the vault.");
        }
    }

    [HttpPost]
    [EnableRateLimiting("vaultLimit")]
    public async Task<IActionResult> Add([FromBody] Vault vault)
    {
        if (vault == null || string.IsNullOrEmpty(vault.VaultId))
            return BadRequest("VaultId is required");
        try
        {
            await _vaultRepository.CreateVaultAsync(vault);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vault");
            return StatusCode(500, "An error occurred while creating the vault.");
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] Vault vault)
    {
        if (vault == null || string.IsNullOrEmpty(vault.VaultId))
            return BadRequest("VaultId is required");
        try
        {
            await _vaultRepository.UpdateVaultAsync(vault);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vault");
            return StatusCode(500, "An error occurred while updating the vault.");
        }
    }
}
