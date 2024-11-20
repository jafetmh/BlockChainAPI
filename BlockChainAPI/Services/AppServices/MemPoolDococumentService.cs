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

namespace BlockChainAPI.Services.AppServices
{
    public class MemPoolDococumentService : IMemPoolDocumentService
    {
        private readonly BlockChainContext _blockchainContext;
        private readonly ILogService _logService;
        private readonly IUserRepository _userRepository;
        private readonly IMempoolRepository _mempoolRepository;
        private readonly IMemPoolDocumentRepository _memPoolDocumentRepository;
        private readonly IGenericDocumentRepository<MemPoolDocument> _genericMemPoolDocumentRepository;
        private readonly Message _message;
        public MemPoolDococumentService(BlockChainContext context,
                                        ILogService logService,
                                        IUserRepository userRepository,
                                        IMempoolRepository mempoolRepository,
                                        IMemPoolDocumentRepository memPoolDocumentRepository,
                                        IGenericDocumentRepository<MemPoolDocument> genericMemPoolDocumentRepository,
                                        MessageService message)
        {
            _blockchainContext = context;
            _logService = logService;
            _userRepository = userRepository;
            _mempoolRepository = mempoolRepository;
            _memPoolDocumentRepository = memPoolDocumentRepository;
            _genericMemPoolDocumentRepository = genericMemPoolDocumentRepository;
            _message = message.Get_Message();
        }

        public async Task<Response<List<MemPoolDocument>>> GetMemPoolDocuments(int userId) => await _memPoolDocumentRepository.GetMempoolDocuments(userId);
        //Insert
        public async Task<Response<MemPoolDocument>> AddMemPoolDocuments(int userId, List<MemPoolDocument> documents)
        {
            try
            {
                Response<User> user = await _userRepository.GetUser(userId);
                Response<MemPool> response = await _mempoolRepository.CreateMempool(userId);
                MemPool memPool = response.Data!; //OjO
                foreach (MemPoolDocument document in documents)
                {
                    document.MemPoolID = memPool.Id;
                    document.CreationDate = document.CreationDate.AddHours(-6);
                }
                await _logService.Log(_message.LogMessages.UploadDocument, user.Data.Name); //log
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

        public async Task<Response<List<MemPoolDocument>>> GetDocumentsByIds(int userId, List<int> documentIds)
        {
            try
            {
                // Obtener todos los documentos del usuario
                var memPoolDocumentsResult = await _memPoolDocumentRepository.GetMempoolDocuments(userId);
                if (!memPoolDocumentsResult.Success)
                    return ResponseResult.CreateResponse<List<MemPoolDocument>>(false, _message.Failure.Get);

                // Filtrar los documentos seleccionados por sus IDs
                var selectedDocuments = memPoolDocumentsResult.Data
                    .Where(doc => documentIds.Contains(doc.Id))
                    .ToList();

                if (!selectedDocuments.Any())
                    return ResponseResult.CreateResponse<List<MemPoolDocument>>(false, "No se encontraron documentos.");

                return ResponseResult.CreateResponse(true, _message.Success.Get, selectedDocuments);
            }
            catch (Exception ex)
            {
                return ResponseResult.CreateResponse<List<MemPoolDocument>>(false, _message.Failure.Get);
            }
        }

        public async Task<List<MemPoolDocument>> FilterMemPoolDocument(int userId, List<MemPoolDocument> documents)
        {
            Response<List<MemPoolDocument>> mempoolDocuments = await GetMemPoolDocuments(userId);
            if (mempoolDocuments.Data == null || !mempoolDocuments.Data.Any()) return documents;

            var docsIds = documents.Select(doc => doc.Id).ToHashSet();
            List<MemPoolDocument> documentsToMine = mempoolDocuments.Data.Where(doc => docsIds.Contains(doc.Id) && !doc.isMined).ToList();
            foreach (MemPoolDocument doc in documentsToMine) {
                doc.isMined = true;
            }
            await _blockchainContext.SaveChangesAsync();
            return documentsToMine;
        }

    }
}
