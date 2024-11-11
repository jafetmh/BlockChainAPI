using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using System.Threading.Tasks;

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

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateBlock(int userId, [FromBody] List<Document> documents)
        {
            Response<Block> responseResult = await _blockService.BuildBlock(userId, documents);
            if (!responseResult.Success) { return StatusCode(500, responseResult); }
            return Ok(responseResult);
        }
    }
}
