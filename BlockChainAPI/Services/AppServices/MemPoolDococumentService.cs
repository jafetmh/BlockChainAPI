using System.Linq;
using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Services.AppServices
{
    public class MemPoolDococumentService : IMemPoolDocumentService
    {
        private readonly IMempoolRepository _mempoolRepository;
        private readonly IMemPoolDocumentRepository _memPoolDocumentRepository;
        private readonly IGenericDocumentRepository<MemPoolDocument> _genericMemPoolDocumentRepository;
        private readonly Message _message;
        public MemPoolDococumentService(IMempoolRepository mempoolRepository,
                                        IMemPoolDocumentRepository memPoolDocumentRepository,
                                        IGenericDocumentRepository<MemPoolDocument> genericMemPoolDocumentRepository,
                                        MessageService message)
        {
            _mempoolRepository = mempoolRepository;
            _memPoolDocumentRepository = memPoolDocumentRepository;
            _genericMemPoolDocumentRepository = genericMemPoolDocumentRepository;
            _message = message.Get_Message();
        }

        public async Task<Response<List<DocumentDTO>>> GetMemPoolDocuments(int userId) => await _memPoolDocumentRepository.GetMempoolDocuments(userId);
        //Insert
        public async Task<Response<MemPoolDocument>> AddMemPoolDocuments(int userId, List<MemPoolDocument> documents)
        {
            try
            {
                Response<MemPool> response = await _mempoolRepository.CreateMempool(userId);
                MemPool memPool = response.Data!; //OjO
                foreach (MemPoolDocument document in documents)
                {
                    document.MemPoolID = memPool.Id;
                    document.CreationDate = document.CreationDate.AddHours(-6);
                }
                return await _genericMemPoolDocumentRepository.BulkCreateDocuments(documents);
            }
            catch (Exception ex) {
                return ResponseResult.CreateResponse<MemPoolDocument>(false, _message.Failure.Set);
            }
        } 
        public async Task<Response<MemPoolDocument>> BulkDeleteMemPoolDocuments(List<MemPoolDocument> documents) => await _genericMemPoolDocumentRepository.BulkDeleteDocument(documents);
        public async Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId) => await _genericMemPoolDocumentRepository.DeleteDocument(documentId);

        public async Task<Response<DocumentDTO>> GetDocumentById(int userId, int documentId)
        {
            try
            {
                return await _memPoolDocumentRepository.GetMempoolDocumentById(userId, documentId);
            }
            catch (Exception ex)
            {
                return ResponseResult.CreateResponse<DocumentDTO>(false, _message.Failure.Get);
            }
        }

        public async Task<Response<List<DocumentDTO>>> GetDocumentsByIds(int userId, List<int> documentIds)
        {
            try
            {
                // Obtener todos los documentos del usuario
                var memPoolDocumentsResult = await _memPoolDocumentRepository.GetMempoolDocuments(userId);
                if (!memPoolDocumentsResult.Success)
                    return ResponseResult.CreateResponse<List<DocumentDTO>>(false, _message.Failure.Get);

                // Filtrar los documentos seleccionados por sus IDs
                var selectedDocuments = memPoolDocumentsResult.Data
                    .Where(doc => documentIds.Contains((int)doc.Id))
                    .ToList();

                if (!selectedDocuments.Any())
                    return ResponseResult.CreateResponse<List<DocumentDTO>>(false, "No se encontraron documentos.");

                return ResponseResult.CreateResponse(true, _message.Success.Get, selectedDocuments);
            }
            catch (Exception ex)
            {
                return ResponseResult.CreateResponse<List<DocumentDTO>>(false, _message.Failure.Get);
            }
        }


    }
}
