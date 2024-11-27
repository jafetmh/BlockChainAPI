using BlockChain_DB;
using BlockChain_DB.General;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;
        }

        // Obtener bloques del usuario
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetBlock(int userId)
        {
            Response<BlockResponse> responseResult = await _blockService.GetBlocks(userId);
            if (!responseResult.Success) return StatusCode(500, responseResult);
            return Ok(responseResult);
        }

        [Authorize]
        [HttpPost("{userId}")]
        public async Task<ActionResult> CreateBlock(int userId, [FromBody] List<MemPoolDocument> documents)
        {
            Response<Block> responseResult = await _blockService.StartMiningTask(userId, documents);
            if (!responseResult.Success) { return StatusCode(500, responseResult); }
            return Ok(responseResult);
        }


    }
}
