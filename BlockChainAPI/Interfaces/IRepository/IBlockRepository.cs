using BlockChain_DB;
using BlockChain_DB.Response;



namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IBlockRepository
    {
        public Task<int> CreateBlock(Block block);
    }
}
