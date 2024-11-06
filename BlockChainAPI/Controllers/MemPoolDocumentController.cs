using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MemPoolDocumentController : ControllerBase
    {

        private readonly IMemPoolDocumentService _memPoolDocumentService;

        public MemPoolDocumentController(IMemPoolDocumentService memPoolDocumentService)
        {
            _memPoolDocumentService = memPoolDocumentService;
        }

        //GET
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<MemPoolDocument>>> GetUserMemPoolDocuments(int userId)
        {
            var result = await _memPoolDocumentService.GetUserMempoolDocuments(userId);
            if (result.Success) {
                return Ok(result);
            }
            return StatusCode(500, result);
        }

        //POST
        [Authorize]
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddMemPoolDocuments(int userId, [FromBody] List<MemPoolDocument> documents)
        {
            var result = await _memPoolDocumentService.AddMemPoolDocuments(userId, documents);
            if (result.Success) { return Ok(result); }
            return StatusCode(500, result);
        }

        //DELETE
        [Authorize]
        [HttpDelete("{documentId}")]
        public async Task<ActionResult> DeleteMemPoolDocument(int documentId)
        {
            var result = await _memPoolDocumentService.DeleteMemPoolDocument(documentId);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }

        public async Task<ActionResult> BulkDetele([FromBody] List<MemPoolDocument> documents)
        {
            Response<MemPoolDocument> result = await _memPoolDocumentService.BulkDelete(documents);
            if (result.Success) { return Ok(result); }
            return StatusCode(500, result);
        }

    }
}
