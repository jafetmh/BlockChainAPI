using BlockChainAPI.Interfaces;
using BlockChainAPI.Interfaces.IMining;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiningController : ControllerBase
    {
        private readonly IMiningService _miningService;

        public MiningController(IMiningService miningService)
        {
            _miningService = miningService;
        }

        [HttpPost("mine")]
        public async Task<IActionResult> MineBlock([FromQuery] int blockId, [FromQuery] int requiredZeros)
        {
            try
            {
                await _miningService.MineBlockAsync(blockId, requiredZeros);
                return Ok("Block mined successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error mining block: {ex.Message}");
            }
        }
    }
}
