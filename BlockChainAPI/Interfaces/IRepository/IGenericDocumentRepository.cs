using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IGenericDocumentRepository<T> where T : class
    {
        Task<Response<T>> BulkCreateDocuments(List<T> documents);
        Task<Response<T>> BulkDeleteDocument(List<T> documents);
        Task<Response<T>> DeleteDocument(int documentId);
    }
}
