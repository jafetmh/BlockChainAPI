using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Utilities;

namespace BlockChainAPI.Repository
{
    public class BlockRepository : IBlockRepository
    {
        private readonly BlockChainContext _context;
        private readonly IChainRepository _chainRepository;

        public BlockRepository(BlockChainContext context, IChainRepository chainRepository)
        {
            _context = context;
            _chainRepository = chainRepository;
        }

        public async Task<Response<List<Block>>> GetBlocks(int userId)
        {
            try
            {
                Response<Chain> responseResult = await _chainRepository.GetChain(userId);
                if (!responseResult.Success || responseResult.Data.Blocks.Count == 0) return ResponseResult.CreateResponse<List<Block>>(false, default);
                return ResponseResult.CreateResponse(true, default, responseResult.Data.Blocks.ToList());
            }
            catch(Exception ex ){ return ResponseResult.CreateResponse<List<Block>>(false, ex.Message); }
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
