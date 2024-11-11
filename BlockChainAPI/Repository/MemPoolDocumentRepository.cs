using BlockChain_DB.DTO;
using BlockChain_DB.General.Message;
using BlockChain_DB;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Utilities;
using BlockChain_DB.Response;

namespace BlockChainAPI.Repository
{
    public class MemPoolDocumentRepository: IMemPoolDocumentRepository
    {
        private readonly IMempoolRepository _mempoolRepository;
        private readonly Message _message;

        public MemPoolDocumentRepository(IMempoolRepository mempoolRepository, Message message) { 
            _mempoolRepository = mempoolRepository;
            _message = message;
        }

        //Get all user mempool docs
        public async Task<Response<List<DocumentDTO>>> GetMempoolDocuments(int userId)
        {

            List<DocumentDTO> documents = new List<DocumentDTO>();
            try
            {
                Response<MemPool> result = await _mempoolRepository.GetMempool(userId);
                MemPool memPool = result.Data;
                if (memPool != null)
                {

                    documents = memPool.Documents
                        .Select(doc => new DocumentDTO
                        {
                            Id = doc.Id,
                            Owner = doc.Owner,
                            FileType = doc.FileType,
                            CreationDate = doc.CreationDate,
                            Size = doc.Size,
                            Doc_encode = doc.Doc_encode,
                        }).ToList();

                }
                return ResponseResult.CreateResponse(true, _message.Success.Get, documents);
            }
            catch { throw; };
        }

        //Get ById
        public async Task<Response<DocumentDTO>> GetMempoolDocumentById(int userId, int documentId)
        {
            try
            {
                Response<MemPool> result = await _mempoolRepository.GetMempool(userId);
                if (!result.Success) { return ResponseResult.CreateResponse<DocumentDTO>(false, _message.Failure.Get); }
                MemPoolDocument document = result.Data.Documents
                        .FirstOrDefault(d => d.Id == documentId);

                if (document != null)
                {
                    var documentDto = new DocumentDTO
                    {
                        Id = document.Id,
                        Owner = document.Owner,
                        FileType = document.FileType,
                        CreationDate = document.CreationDate,
                        Size = document.Size,
                        Doc_encode = document.Doc_encode,
                    };

                    return ResponseResult.CreateResponse(true, _message.Success.Get, documentDto);
                }
                return ResponseResult.CreateResponse<DocumentDTO>(false, _message.Failure.Get);
            }
            catch { throw; }
        }
    }
}
