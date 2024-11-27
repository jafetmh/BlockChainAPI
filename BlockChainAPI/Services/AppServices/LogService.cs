using BlockChain_DB;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using System.Text.Json;

namespace BlockChainAPI.Services.AppServices
{
    public class LogService : ILogService
    {
        private readonly BlockChainContext _context;
        public LogService(BlockChainContext context) { 
            _context = context;
        }
        public async Task Log(string action, string user, object details = null)
        {
            SystemLog newLog = new SystemLog()
            {
                Action = action,
                User = user,
                Details = details != null?JsonSerializer.Serialize(details) : null
            };
            _context.SystemLog.Add(newLog);
            await _context.SaveChangesAsync();
        }
    }
}
