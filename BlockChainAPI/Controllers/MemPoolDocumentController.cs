using BlockChain_DB;
using BlockChain_DB.DTO;
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
        [HttpDelete("{userId}/{documentId}")]
        public async Task<ActionResult> DeleteMemPoolDocument(int userId, int documentId)
        {
            var result = await _memPoolDocumentService.DeleteMemPoolDocument(userId, documentId);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }

        [HttpGet("{userId}/{documentId}")]
        public async Task<ActionResult<MemPoolDocumentDTO>> GetDocumentById(int userId, int documentId)
        {
            try
            {
                // Usamos el servicio para obtener el documento
                var result = await _memPoolDocumentService.GetDocumentById(userId, documentId);

                // Verificamos si la operación fue exitosa
                if (result.Success)
                {
                    return Ok(result.Data);  // Retornamos el documento encontrado
                }
                else
                {
                    return NotFound(result.Message);  // Retornamos un mensaje si no se encuentra el documento
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Retornamos error si ocurre alguna excepción
            }
        }




    }
}
