using BlockChain_DB;

namespace BlockChainAPI.Interfaces.IRepository
{
    public interface IDocumentRepository
    {
        public Task BulkCreateDocuments(List<Document> documents);

    }
}
