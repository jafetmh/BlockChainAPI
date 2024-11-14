using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IRepository
{
    public interface IMempoolRepository
    {
        Task<Response<MemPool>> GetMempool(int userId);
        Task<Response<MemPool>> CreateMempool(int userId);
    }
}
