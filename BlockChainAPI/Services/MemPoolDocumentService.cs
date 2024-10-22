using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Utilities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Services
{
    public class MemPoolDocumentService
    {

        private readonly BlockChainContext _context;

        public MemPoolDocumentService(BlockChainContext context)
        {
            _context = context;
        }

        // get
        public async Task<List<MemPoolDocument>> GetUserMempoolDocuments(int userId)
        {
            var memPool = await _context.MemPools
                            .Include(mp =>mp.Documents)
                            .FirstOrDefaultAsync(mp => mp.UserID == userId);

            return memPool?.Documents.ToList()?? new List<MemPoolDocument>();
        }

        //add bulk of documents 
        public async Task AddMemPoolDocuments(int userId, List<MemPoolDocument> documents)
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
                }
                await _context.BulkInsertAsync(documents);
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }

        }

        //remove document
        public async Task<Response<MemPoolDocument>> RemoveMemPoolDocument(int documentId)
        {
            var document = await _context.MemPoolDocuments.FindAsync(documentId);
            if (document != null) {
                _context.MemPoolDocuments.Remove(document);
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, "Eliminado correctamente", document);
            }
            return ResponseResult.CreateResponse<MemPoolDocument>(false, "No encontrado");

        }
    }
}
