using BlockChain_DB;

namespace BlockChainAPI.Interfaces.IRepository
{
    public interface IChainRepository
    {
        public Task<Chain> GetUserChain(int userId);
    }
}
