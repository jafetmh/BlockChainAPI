
using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces
{
    public interface IMemPoolDocumentService
    {
        public Task<List<MemPoolDocumentDTO>> GetUserMempoolDocuments(int userId);
        public Task AddMemPoolDocuments(int userId, List<MemPoolDocument> documents);
        public Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId);

        //question for the futur
        //public Task<Response<MemPoolDocument>> UpdateMemPoolDocument(int documentId, MemPoolDocument document);
        //public Task<Response<MemPoolDocument>> GetMemPoolDocument(int documentId);
    }
}