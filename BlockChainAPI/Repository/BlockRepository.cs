using BlockChain_DB;
using BlockChainAPI.Interfaces.IDataService;

namespace BlockChainAPI.Repository
{
    public class BlockRepository : IBlockRepository
    {
        private readonly BlockChainContext _context;

        public BlockRepository(BlockChainContext context)
        {
            _context = context;
        }

        public async Task<int> CreateBlock(Block block)
        {
            try
            {
                _context.Blocks.Add(block);
                return await _context.SaveChangesAsync();
            } catch { throw; }
        }
    }
}
