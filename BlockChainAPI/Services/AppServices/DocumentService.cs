using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using BlockChainAPI.Interfaces.IServices.ICrypto.AES;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;

namespace BlockChainAPI.Services.AppServices
{
    public class DocumentService : IDocumentService
    {
        private readonly IGenericDocumentRepository<Document> _documentRepository;
        private readonly IAESEncryption _encryption;
        private readonly IDocumentRepository _documentRepository1;
        private readonly Message _message;
        public DocumentService(IGenericDocumentRepository<Document> documentRespository,
                               IAESEncryption encryption,
                               IDocumentRepository documentRepository1,
                               MessageService message)
        {
            _documentRepository = documentRespository;
            _encryption = encryption;
            _documentRepository1 = documentRepository1;
            _message = message.Get_Message();
        }

        //Document services
        public async Task<Response<Document>> BulkCreateDocuments(User user, List<Document> documents, Block block)
        {
            try
            {
                int id = 0;
                _encryption.GetKeyAndIv(user);
                foreach (Document document in documents)
                {
                    document.Id = id;
                    document.BlockID = block.Id;
                    document.CreationDate = document.CreationDate.AddHours(-6);
                    document.Doc_encode = await _encryption.EncryptDocument(document.Doc_encode);
                    id++;
                }
                 await _documentRepository1.BulkCreateDocuments(documents);
                return ResponseResult.CreateResponse<Document>(true, default);
            }
            catch (Exception ex) {
                return ResponseResult.CreateResponse<Document>(false, ex.Message);
            }

        }
        public async Task<Response<Document>> BulkDeleteDocuments(List<Document> documents) => await _documentRepository.BulkDeleteDocument(documents);
        public async Task<Response<Document>> DeleteDocument(int documentId) => await _documentRepository.DeleteDocument(documentId);

    }
}
