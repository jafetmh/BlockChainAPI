using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Repository
{
    public class ChainRepository: IChainRepository
    {
        private readonly BlockChainContext _context;
        public ChainRepository(BlockChainContext context) { _context = context; }


     
        public async Task<Response<Chain>> GetChain(int userId)
        {
            Chain chain = await _context.Chains
                .Where(ch => ch.UserID == userId)
                .Include(ch => ch.Blocks)
                .ThenInclude(b => b.Documents)
                .AsNoTracking() 
                .FirstOrDefaultAsync();

            if (chain == null) return ResponseResult.CreateResponse<Chain>(false, default);
            return ResponseResult.CreateResponse(true, default, chain);
        }

        public async Task<Chain> CreateChain(int userId)
        {
            Response<Chain> responseResult = await GetChain(userId);
            Chain chain = responseResult.Data;

            if (chain == null)
            {
                chain = new Chain { UserID = userId };
                _context.Chains.Add(chain);
                await _context.SaveChangesAsync();
            }
            return chain;
        }

    }
}
