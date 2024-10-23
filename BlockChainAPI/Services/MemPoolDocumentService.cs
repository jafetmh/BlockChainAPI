using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Utilities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Services
{
    public class MemPoolDocumentService: IMemPoolDocumentService
    {

        private readonly BlockChainContext _context;

        public MemPoolDocumentService(BlockChainContext context)
        {
            _context = context;
        }

        // get
        public async Task<List<MemPoolDocumentDTO>> GetUserMempoolDocuments(int userId)
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

                    return documents;
                }
                return documents;
            }
            catch
            {
                Console.WriteLine("Error al obtner documentos");
                return documents;
            }
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
                await _context.BulkInsertAsync(documents);//library for bulk insert documents
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }

        }

        //remove document
        public async Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId)
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
