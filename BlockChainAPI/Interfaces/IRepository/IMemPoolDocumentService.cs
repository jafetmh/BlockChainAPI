
using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IMemPoolDocumentService
    {
        //public Task<Response<List<DocumentDTO>>> GetUserMempoolDocuments(int userId);
        public Task<Response<MemPoolDocument>> AddMemPoolDocuments(int userId, List<MemPoolDocument> documents);
        public Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId);
        public Task<Response<MemPoolDocument>> BulkDelete(List<MemPoolDocument> documents);
        public Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int userId, int documentId);
        Task<Response<DocumentDTO>> GetDocumentById(int userId, int documentId);
    }
}