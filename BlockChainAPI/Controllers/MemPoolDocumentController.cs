using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IServices.IAppServices;
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
            var result = await _memPoolDocumentService.GetMemPoolDocuments(userId);
            if (result.Success) return StatusCode(500, result);
            return Ok(result);

        }

        //POST
        [Authorize]
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddMemPoolDocuments(int userId, [FromBody] List<MemPoolDocument> documents)
        {
            var result = await _memPoolDocumentService.AddMemPoolDocuments(userId, documents);
            if (!result.Success) return StatusCode(500, result);
            return Ok(result);

        }

        //BULK Delete
        [Authorize]
        [HttpDelete("/bulkdelete")]
        public async Task<ActionResult> BulkDelete([FromBody] List<MemPoolDocument> documents)
        {
            Response<MemPoolDocument> result = await _memPoolDocumentService.BulkDeleteMemPoolDocuments(documents);
            if (!result.Success) return StatusCode(500, result);
            return Ok(result);

        }

        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteMemPoolDocument(int documentId)
        {
            Response<MemPoolDocument> responseResult = await _memPoolDocumentService.DeleteMemPoolDocument(documentId);
            if(!responseResult.Success) return StatusCode(500, responseResult);
            return Ok(responseResult);
        }

        //DELETE
        //[Authorize]
        //[HttpDelete("{userId}/{documentId}")]
        //public async Task<ActionResult> DeleteMemPoolDocument(int userId, int documentId)
        //{
        //    var result = await _memPoolDocumentService.DeleteMemPoolDocument(userId, documentId);
        //    if (result.Success) { return Ok(result); }
        //    return BadRequest(result);
        //}

        //[HttpGet("{userId}/{documentId}")]
        //public async Task<ActionResult<DocumentDTO>> GetDocumentById(int userId, int documentId)
        //{
        //    try
        //    {
        //        var result = await _memPoolDocumentService.GetDocumentById(userId, documentId);

        //        if (result.Success)
        //        {
        //            return Ok(result.Data);
        //        }
        //        return NotFound(result.Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

    }
}
