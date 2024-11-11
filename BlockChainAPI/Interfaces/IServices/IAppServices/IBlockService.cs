using BlockChain_DB;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IServices.IAppServices
{
    public interface IBlockService
    {
        public Task<Response<Block>> BuildBlock(int userId, List<Document> documents);
        public Task MineBlock(Block block, User user, string docsBase64);
        public Task<string> GenerateHash(string data, User user);
        public string GetDocsBase64tring(List<Document> documents);

    }
}
