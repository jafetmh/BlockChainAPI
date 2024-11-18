using BlockChain_DB;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IRepository
{
    public interface IChainRepository
    {
        public Task<Response<Chain>> GetChain(int userId);
        public Task<Chain> CreateChain(int userId);
    }
}
