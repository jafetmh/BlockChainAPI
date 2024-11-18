using BlockChain_DB;
using BlockChain_DB.General;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IServices.IAppServices
{
    public interface IBlockService
    {
        public Task<Response<Block>> StartMiningTask(int userId, List<Document> documents);
        public Task<Response<Block>> BuildBlock(int userId, List<Document> documents);
        public void MiningBlock(Block block, User user, string docsBase64);
        public string GetDocsBase64tring(List<Document> documents);
        public Task<Response<BlockResponse>> GetBlocks(int userId);
        public List<Block> VerifyBlockHashesConsistency(List<Block> blocks);
        public Task<List<Block>> VerifyBlockIntegrity(List<Block> blocks);
        public Task DecryptDocuments(List<Document> documents);
    }
}
