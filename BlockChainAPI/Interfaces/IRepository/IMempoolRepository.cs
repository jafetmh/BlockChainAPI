using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IRepository
{
    public interface IMempoolRepository
    {
        Task<Response<MemPool>> GetUserMempool(int userId);
        Task<Response<MemPool>> CreateMempool(int userId);
        Task<Response<List<DocumentDTO>>> GetUserMempoolDocuments(int userId);
        Task<Response<DocumentDTO>> GetMempoolDocumentById(int userId, int documentId);

    }
}
