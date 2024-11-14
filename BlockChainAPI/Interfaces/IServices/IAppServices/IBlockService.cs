using BlockChain_DB;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IServices.IAppServices
{
    public interface IBlockService
    {
        public Task<Response<Block>> StartMiningTask(int userId, List<Document> documents);
        public Task<Response<Block>> BuildBlock(int userId, List<Document> documents);
        public void MiningBlock(Block block, User user, string docsBase64);
        public string GetDocsBase64tring(List<Document> documents);
        public Task<Response<List<Block>>> GetBlocks(int userId);

    }
}
