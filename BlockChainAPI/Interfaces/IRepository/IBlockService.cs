using BlockChain_DB;
using BlockChain_DB.Response;



namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IBlockService
    {
        public Task<Response<Block>> BuildBlock(User user, List<Document> documents);
        public Task MineBlock(Block block, User user, string docsBase64);
        public Task<string> GenerateHash(string data, User user);
        public string GetDocsBase64tring(List<Document> documents);
        public Task<Chain> GetUserChain(int userId);
    }
}
