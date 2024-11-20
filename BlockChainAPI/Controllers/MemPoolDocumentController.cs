using System.IO.Compression;
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
            if (!result.Success) return StatusCode(500, result);
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
        [HttpDelete("bulkdelete")]
        public async Task<ActionResult> BulkDelete([FromBody] List<MemPoolDocument> documents)
        {
            Response<MemPoolDocument> result = await _memPoolDocumentService.BulkDeleteMemPoolDocuments(documents);
            if (!result.Success) return StatusCode(500, result);
            return Ok(result);

        }

        //DELETE
        [Authorize]
        [HttpDelete("{documentId}")]
        public async Task<ActionResult> DeleteMemPoolDocument(int documentId)
        {
            Response<MemPoolDocument> responseResult = await _memPoolDocumentService.DeleteMemPoolDocument(documentId);
            if(!responseResult.Success) return StatusCode(500, responseResult);
            return Ok(responseResult);
        }


        [Authorize]
        [HttpGet("{userId}/document/{documentId}")]
        public async Task<ActionResult<DocumentDTO>> GetDocumentById(int userId, int documentId)
        {
            var result = await _memPoolDocumentService.GetDocumentById(userId, documentId);
            if (!result.Success) return StatusCode(500, result);
            return Ok(result);
        }

        // Endpoint para descargar múltiples documentos, solo devolviendo los IDs y sus respectivos datos base64
        [Authorize]
        [HttpGet("{userId}/documents/zip")]
        public async Task<ActionResult> GetDocumentsByIds(int userId, [FromQuery] List<int> documentIds)
        {
            if (documentIds == null || !documentIds.Any())
                return BadRequest("Debe seleccionar al menos un documento.");

            // Obtener los documentos por ID
            var documentsResult = await _memPoolDocumentService.GetDocumentsByIds(userId, documentIds);

            if (!documentsResult.Success)
                return StatusCode(500, documentsResult);

            var documents = documentsResult.Data;

            // Crear un objeto de respuesta que solo incluye los IDs y los datos base64
            var documentResponses = documents.Select(doc => new
            {
                doc.Id,
                doc.Owner,
                doc.FileType,
                doc.Doc_encode
            }).ToList();
            return Ok(documentResponses);
        }
    }
}

