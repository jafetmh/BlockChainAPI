using BlockChain_BD;
using BlockChain_BD.Response;

namespace BlockChainAPI.Interfaces
{
    public interface IMemPoolDocumentService
    {
        public Task<List<MemPoolDocument>> GetUserMempoolDocuments(int userId);
        public Task AddMemPoolDocuments(int userId, List<MemPoolDocument> documents);
        public Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId);

        //question for the futur
        //public Task<Response<MemPoolDocument>> UpdateMemPoolDocument(int documentId, MemPoolDocument document);
        //public Task<Response<MemPoolDocument>> GetMemPoolDocument(int documentId);
    }
}