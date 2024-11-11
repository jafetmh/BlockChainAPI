using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IServices.IAppServices;

namespace BlockChainAPI.Services.AppServices
{
    public class DocumentService: IDocumentService
    {

        private readonly IGenericDocumentRepository<Document> _documentRespository;

        public DocumentService(IGenericDocumentRepository<Document> documentRespository, IGenericDocumentRepository<MemPoolDocument> memPoolDocumentRespository)
        {
            _documentRespository = documentRespository;
        }

        //Document services
        public async Task<Response<Document>> BulkCreateDocuments(int userId, List<Document> documents, Block block)
        {
            foreach (Document document in documents)
            {
                document.BlockID = block.Id;
                document.CreationDate = document.CreationDate.AddHours(-6);
            }
            return await _documentRespository.BulkCreateDocuments(userId, documents);
        }
        public async Task<Response<Document>> BulkDeleteDocuments(List<Document> documents) => await _documentRespository.BulkDeleteDocument(documents);
        public async Task<Response<Document>> DeleteDocument(int documentId) => await _documentRespository.DeleteDocument(documentId);

    }
}
