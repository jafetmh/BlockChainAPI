using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IRepository
{
    public interface IMemPoolDocumentRepository
    {
        Task<Response<List<MemPoolDocument>>> GetMempoolDocuments(int userId);
        Task<Response<DocumentDTO>> GetMempoolDocumentById(int userId, int documentId);

    }
}
