using BlockChain_DB;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IServices.IAppServices
{
    public interface IDocumentService
    {
        public Task<Response<Document>> BulkCreateDocuments(int userId, List<Document> documents, Block block);
        public Task<Response<Document>> BulkDeleteDocuments(List<Document> documents);
        public Task<Response<Document>> DeleteDocument(int documentId);

    }
}
