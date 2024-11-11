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
        public async Task<Response<MemPool>> GetMempool(int userId)
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
                Response<MemPool> result = await GetMempool(userId);
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

    }
}
