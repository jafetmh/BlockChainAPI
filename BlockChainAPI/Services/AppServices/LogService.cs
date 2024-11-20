using BlockChain_DB;
using BlockChainAPI.Interfaces.IServices.IAppServices;

namespace BlockChainAPI.Services.AppServices
{
    public class LogService : ILogService
    {
        private readonly BlockChainContext _context;
        public LogService(BlockChainContext context) { 
            _context = context;
        }
        public async Task Log(string action, string user, string details = null)
        {
            SystemLog newLog = new SystemLog()
            {
                Action = action,
                User = user,
                Details = details
            };
            _context.SystemLog.Add(newLog);
            await _context.SaveChangesAsync();
        }
    }
}
