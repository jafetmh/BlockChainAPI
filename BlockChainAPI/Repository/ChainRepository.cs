using BlockChain_DB;
using BlockChainAPI.Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Repository
{
    public class ChainRepository: IChainRepository
    {
        private readonly BlockChainContext _context;
        public ChainRepository(BlockChainContext context) { _context = context; }


        //Get or create a user chain
        public async Task<Chain> GetUserChain(int userId)
        {
            try
            {
                Chain chain = await _context.Chains
                    .Include(ch => ch.Blocks)
                    .FirstOrDefaultAsync(ch => ch.UserID == userId);

                if (chain == null)
                {
                    chain = new Chain { UserID = userId };
                    _context.Chains.Add(chain);
                    await _context.SaveChangesAsync();
                }
                return chain;
            }
            catch { throw; }
        }
    }
}
