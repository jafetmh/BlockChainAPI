using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Repository
{
    public class MempoolRepository : IMempoolRepository
    {
        private readonly BlockChainContext _context;
        private readonly Message _message;

        public MempoolRepository(BlockChainContext context, MessageService messages)
        {
            _context = context;
            _message = messages.Get_Message();
        }

        //Get
        public async Task<Response<MemPool>> GetUserMempool(int userId)
        {
            try
            {
                MemPool memPool = await _context.MemPools
                    .Include(mp => mp.Documents)
                    .FirstOrDefaultAsync(mp => mp.UserID == userId);

                if (memPool == null) { return ResponseResult.CreateResponse<MemPool>(false, _message.NotFound); }
                return ResponseResult.CreateResponse(true, _message.Success.Get, memPool);
            }
            catch { throw; }

        }

        //Create
        public async Task<Response<MemPool>> CreateMempool(int userId)
        {
            try
            {
                Response<MemPool> result = await GetUserMempool(userId);
                if (result.Data == null)
                {

                    result.Data = new MemPool { UserID = userId };
                    _context.MemPools.Add(result.Data);
                    await _context.SaveChangesAsync();
                }
                return result;
            }
            catch { throw; }
        }

        //Get all user mempool docs
        public async Task<Response<List<DocumentDTO>>> GetUserMempoolDocuments(int userId)
        {

            List<DocumentDTO> documents = new List<DocumentDTO>();
            try
            {
                Response<MemPool> result = await GetUserMempool(userId);
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
                Response<MemPool> result = await GetUserMempool(userId);
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
