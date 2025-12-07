using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VaultController : ControllerBase
{

    private readonly IVaultRepository _vaultRepository;

    public VaultController(IVaultRepository vaultRepository)
    {
        _vaultRepository = vaultRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("works man");
    }

    [HttpGet("vaultId")]
    public async Task<IActionResult> GetVault([FromBody] string vaultId)
    {
        var vault = await _vaultRepository.GetVaultAsync(vaultId);
        return Ok(vault);
    }

    [HttpGet("all")]
    public async Task<List<Vault>> GetAll()
    {
        return await _vaultRepository.GetAllVaultAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Vault vault)
    {
        if (vault == null || string.IsNullOrEmpty(vault.VaultId))
        {
            return BadRequest("VaultId required .");
        }
        await _vaultRepository.CreateVaultAsync(vault);
        return Ok(new { success = true });
    }

}