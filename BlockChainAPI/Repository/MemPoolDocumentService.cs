using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Repository
{
    public class MemPoolDocumentService : IMemPoolDocumentService
    {

        private readonly BlockChainContext _context;
        private readonly Message message;

        public MemPoolDocumentService(BlockChainContext context, MessageService messages)
        {
            _context = context;
            message = messages.Get_Message();
        }



        //bulk create
        //public async Task<Response<MemPoolDocument>> AddMemPoolDocuments(int userId, List<MemPoolDocument> documents)
        //{
        //    try
        //    {
        //        var memPool = await _context.MemPools
        //        .Include(mp => mp.Documents)
        //        .FirstOrDefaultAsync(mp => mp.UserID == userId);

        //        if (memPool == null)
        //        {
        //            memPool = new MemPool { UserID = userId };
        //            _context.MemPools.Add(memPool);
        //            await _context.SaveChangesAsync();
        //        }

        //        foreach (MemPoolDocument document in documents)
        //        {
        //            document.MemPoolID = memPool.Id;
        //            document.CreationDate = document.CreationDate.AddHours(-6);
        //        }
        //        await _context.BulkInsertAsync(documents);
        //        return ResponseResult.CreateResponse<MemPoolDocument>(true, message.Success.Set);
        //    }
        //    catch (Exception ex) { 
        //        Console.WriteLine(ex.Message);
        //        return ResponseResult.CreateResponse<MemPoolDocument>(true, message.Failure.Set);
        //    }

        //}

        //remove a document
        //public async Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int documentId)
        //{
        //    var document = await _context.MemPoolDocuments.FindAsync(documentId);
        //    if (document != null) {
        //        _context.MemPoolDocuments.Remove(document);
        //        await _context.SaveChangesAsync();
        //        return ResponseResult.CreateResponse(true, message.Success.Remove, document);
        //    }
        //    return ResponseResult.CreateResponse<MemPoolDocument>(false, message.NotFound);

        //}

        //bulk delete doc
        //public async Task<Response<MemPoolDocument>> BulkDelete(List<MemPoolDocument> documents)
        //{
        //    try
        //    {
        //        await _context.BulkDeleteAsync(documents);
        //        return ResponseResult.CreateResponse<MemPoolDocument>(true, message.Success.Remove);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return ResponseResult.CreateResponse<MemPoolDocument>(false, message.Failure.Remove);
        //    }
        //}

        //Delete one doc
        public async Task<Response<MemPoolDocument>> DeleteMemPoolDocument(int userId, int documentId)
        {
            try
            {
                var memPool = await _context.MemPools
                                    .Include(mp => mp.Documents)
                                    .FirstOrDefaultAsync(mp => mp.UserID == userId);

                if (memPool == null)
                {
                    return ResponseResult.CreateResponse<MemPoolDocument>(false, message.NotFound);
                }

                var document = memPool.Documents.FirstOrDefault(d => d.Id == documentId);
                if (document != null)
                {
                    _context.MemPoolDocuments.Remove(document);
                    await _context.SaveChangesAsync();
                    return ResponseResult.CreateResponse(true, message.Success.Remove, document);
                }
                return ResponseResult.CreateResponse<MemPoolDocument>(false, message.NotFound);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseResult.CreateResponse<MemPoolDocument>(false, message.Failure.Remove);
            }
        }

        //public async Task<Response<DocumentDTO>> GetDocumentById(int userId, int documentId)
        //{
        //    try
        //    {
        //        var memPool = await _context.MemPools
        //                        .Include(mp => mp.Documents)
        //                        .FirstOrDefaultAsync(mp => mp.UserID == userId);

        //        if (memPool != null)
        //        {
        //           var document = memPool.Documents
        //                                   .FirstOrDefault(d => d.Id == documentId);

        //            if (document != null)
        //            {
        //               var documentDto = new DocumentDTO
        //                {
        //                    Id = document.Id,
        //                    Owner = document.Owner,
        //                    FileType = document.FileType,
        //                    CreationDate = document.CreationDate,
        //                    Size = document.Size,
        //                    Doc_encode = document.Doc_encode,
        //                };

        //                return ResponseResult.CreateResponse(true, message.Success.Get, documentDto);
        //            }
        //            return ResponseResult.CreateResponse<DocumentDTO>(false, message.Failure.Get);
        //        }
        //        return ResponseResult.CreateResponse<DocumentDTO>(false, message.Failure.Get);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseResult.CreateResponse<DocumentDTO>(false, message.Failure.Get + ex.Message);
        //    }
        //}

    }
}
