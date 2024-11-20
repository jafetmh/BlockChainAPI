using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IServices.IAppServices
{
    public interface IMemPoolDocumentService
    {
        public Task<Response<List<MemPoolDocument>>> GetMemPoolDocuments(int userId);
        public Task<Response<MemPoolDocument>> AddMemPoolDocuments(int userId, List<MemPoolDocument> documents);
        public Task<Response<MemPoolDocument>> BulkDeleteMemPoolDocuments(List<MemPoolDocument> documents);
        public Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId);
        Task<Response<DocumentDTO>> GetDocumentById(int userId, int documentId);
        Task<Response<List<MemPoolDocument>>> GetDocumentsByIds(int userId, List<int> documentIds);
        Task<List<MemPoolDocument>> FilterMemPoolDocument(int userId, List<MemPoolDocument> documents);

    }
}
