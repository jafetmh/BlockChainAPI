
using BlockChain_DB;

namespace BlockChainAPI.Services
{
    public class Configuration_Service
    {
        private readonly BlockChainContext _context;

        public Configuration_Service(BlockChainContext context) 
        { 
            _context = context;
        }

        public int GetMaxBlockDocuments()
        {
            var sys = new SystemConfig();
           // var config = _context.SystemConfig.Fir
            return 0;
                
        }
    }
}
