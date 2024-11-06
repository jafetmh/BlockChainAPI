using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Services
{
    public class MemPoolDocumentService: IMemPoolDocumentService
    {

        private readonly BlockChainContext _context;
        private readonly Message message;

        public MemPoolDocumentService(BlockChainContext context, MessageService messages)
        {
            _context = context;
            message = messages.Get_Message();
        }

        // get
        public async Task<Response<List<MemPoolDocumentDTO>>> GetUserMempoolDocuments(int userId)
        {

            List<MemPoolDocumentDTO> documents = new List<MemPoolDocumentDTO>();
            try
            {
                var memPool = await _context.MemPools
                           .Include(mp => mp.Documents)
                           .FirstOrDefaultAsync(mp => mp.UserID == userId);

                if (memPool != null)
                {

                    documents = memPool.Documents
                        .Select(doc => new MemPoolDocumentDTO
                        {
                            Id = doc.Id,
                            Owner = doc.Owner,
                            FileType = doc.FileType,
                            CreationDate = doc.CreationDate,
                            Size = doc.Size,
                            Doc_encode = doc.Doc_encode,
                        }).ToList();

                }
                return ResponseResult.CreateResponse(true, message.Success.Get, documents);
            }
            catch
            {
                return ResponseResult.CreateResponse(false, message.Failure.Get, documents);
            }
        }

        //bulk create
        public async Task<Response<MemPoolDocument>> AddMemPoolDocuments(int userId, List<MemPoolDocument> documents)
        {
            try
            {
                var memPool = await _context.MemPools
                .Include(mp => mp.Documents)
                .FirstOrDefaultAsync(mp => mp.UserID == userId);

                if (memPool == null)
                {
                    memPool = new MemPool { UserID = userId };
                    _context.MemPools.Add(memPool);
                    await _context.SaveChangesAsync();
                }

                foreach (MemPoolDocument document in documents)
                {
                    document.MemPoolID = memPool.Id;
                    document.CreationDate = document.CreationDate.AddHours(-6);
                }
                await _context.BulkInsertAsync(documents);
                return ResponseResult.CreateResponse<MemPoolDocument>(true, message.Success.Set);
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                return ResponseResult.CreateResponse<MemPoolDocument>(true, message.Failure.Set);
            }

        }

        //remove document
        public async Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId)
        {
            var document = await _context.MemPoolDocuments.FindAsync(documentId);
            if (document != null) {
                _context.MemPoolDocuments.Remove(document);
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, message.Success.Remove, document);
            }
            return ResponseResult.CreateResponse<MemPoolDocument>(false, message.NotFound);

        }
    }
}
