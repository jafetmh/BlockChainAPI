using BlockChain_DB.General.Message;
using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using BlockChainAPI.Utilities.ResponseMessage;
using BlockChain_DB.DTO;

namespace BlockChainAPI.Repository
{
    public class GenericDocumentRepository<T> : IGenericDocumentRepository<T> where T : class
    {
        private readonly BlockChainContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly Message message;

        public GenericDocumentRepository(BlockChainContext context, MessageService messages)
        {
            _context = context;
            _dbSet = _context.Set<T>(); //allow repo access to specific Entity
            message = messages.Get_Message();
        }


        //bulk create
        public async Task<Response<T>> BulkCreateDocuments(int userId, List<T> documents)
        {
            try
            {

                //foreach (MemPoolDocument document in documents)
                //{
                //    document.MemPoolID = memPool.Id;
                //    document.CreationDate = document.CreationDate.AddHours(-6);
                //}
                await _context.BulkInsertAsync(documents);
                return ResponseResult.CreateResponse<T>(true, message.Success.Set);
            }
            catch (Exception ex)
            {
                return ResponseResult.CreateResponse<T>(true, ex.Message);
            }
        }

        //bulk delete
        public async Task<Response<T>> BulkDeleteDocument(List<T> documents)
        {
            try
            {
                await _context.BulkDeleteAsync(documents);
                return ResponseResult.CreateResponse<T>(true, message.Success.Remove);
            }
            catch (Exception ex)
            {
                return ResponseResult.CreateResponse<T>(false, ex.Message);
            }
        }

        //delete one document
        public async Task<Response<T>> DeleteDocument(int documentId)
        {
            var document = await _dbSet.FindAsync(documentId);
            if (document != null)
            {
                _dbSet.Remove(document);
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, message.Success.Remove, document);
            }
            return ResponseResult.CreateResponse<T>(false, message.NotFound);
        }

    }
}
