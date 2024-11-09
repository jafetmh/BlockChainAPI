using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using System.Collections.Generic;

namespace BlockChainAPI.Services
{
    public class DocumentService
    {

        private readonly IGenericDocumentRepository<Document> _documentRespository;
        private readonly IGenericDocumentRepository<MemPoolDocument> _memPoolDocumentRespository;

        public DocumentService(IGenericDocumentRepository<Document> documentRespository, IGenericDocumentRepository<MemPoolDocument> memPoolDocumentRespository)
        {
            _documentRespository = documentRespository;
            _memPoolDocumentRespository = memPoolDocumentRespository;
        }

        //Document data acces methods
        public Task<Response<Document>> BulkCreateDocuments(int userId, List<Document> documents) => _documentRespository.BulkCreateDocuments(userId, documents);
        public Task<Response<Document>> BulkDeleteDocuments(List<Document> documents) => _documentRespository.BulkDeleteDocument(documents);
        public Task<Response<Document>> DeleteDocument(int documentId) => _documentRespository.DeleteDocument(documentId);

        //MemPoolDocument
        public Task<Response<MemPoolDocument>> BulkCreateMemPoolDocuments(int userId, List<MemPoolDocument> documents) => _memPoolDocumentRespository.BulkCreateDocuments(userId, documents);
        public Task<Response<MemPoolDocument>> BulkDeleteMemPoolDocuments(List<MemPoolDocument> documents) => _memPoolDocumentRespository.BulkDeleteDocument(documents);
        public Task<Response<MemPoolDocument>> DeleteMemPoolDocuments(int documentId) => _memPoolDocumentRespository.DeleteDocument(documentId);
    } 
}
