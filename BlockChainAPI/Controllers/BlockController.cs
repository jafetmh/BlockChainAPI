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

        // Verificar la consistencia de los hashes entre los bloques
        [Authorize]
        [HttpGet("verify-hash-consistency/{userId}")]
        public async Task<ActionResult> VerifyHashConsistency(int userId)
        {
            try
            {
                Response<BlockResponse> responseResult = await _blockService.GetBlocks(userId);
                if (!responseResult.Success) return StatusCode(500, responseResult);

                List<Block> inconsistentBlocks = _blockService.VerifyBlockHashesConsistency(responseResult.Data.Blocks);

                return Ok(new
                {
                    Success = true,
                    Message = inconsistentBlocks.Count > 0 ? "Inconsistent hashes found" : "All hashes are consistent",
                    Data = inconsistentBlocks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        // Verificar la integridad de los bloques
        [Authorize]
        [HttpGet("verify-block-integrity/{userId}")]
        public async Task<ActionResult> VerifyBlockIntegrity(int userId)
        {
            try
            {
                Response<BlockResponse> responseResult = await _blockService.GetBlocks(userId);
                if (!responseResult.Success) return StatusCode(500, responseResult);

                List<Block> alteredBlocks = await _blockService.VerifyBlockIntegrity(responseResult.Data.Blocks);

                return Ok(new
                {
                    Success = true,
                    Message = alteredBlocks.Count > 0 ? "Altered blocks found" : "All blocks are intact",
                    Data = alteredBlocks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        // Desencriptar documentos de un bloque
        [Authorize]
        [HttpPost("decrypt-documents")]
        public async Task<ActionResult> DecryptDocuments([FromBody] List<Document> documents)
        {
            try
            {
                await _blockService.DecryptDocuments(documents);
                return Ok(new { Success = true, Message = "Documents decrypted successfully", Data = documents });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

    }
}
