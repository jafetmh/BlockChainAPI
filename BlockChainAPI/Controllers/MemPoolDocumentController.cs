using BlockChain_DB;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
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
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<MemPoolDocument>>> GetUserMemPoolDocuments(int userId)
        {
            var result = await _memPoolDocumentService.GetUserMempoolDocuments(userId);
            if (result.Data.Count != 0) {
                return Ok(result);
            }
            return NoContent();
        }

        //POST
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddMemPoolDocuments(int userId, [FromBody] List<MemPoolDocument> documents)
        {
            await _memPoolDocumentService.AddMemPoolDocuments(userId, documents);
            return NoContent();
        }

        //DELETE
        [HttpDelete("{documentId}")]
        public async Task<ActionResult> DeleteMemPoolDocument(int documentId)
        {
            var result = await _memPoolDocumentService.DeleteMemPoolDocument(documentId);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }

    }
}
