
using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Repository
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly BlockChainContext _context;
        private readonly Message message;


        public ConfigurationRepository(BlockChainContext context, MessageService messages)
        {
            _context = context;
            message = messages.Get_Message();
        }

        //get
        public async Task<Response<SystemConfig>> GetMaxBlockDocuments()
        {
            SystemConfig config = await _context.SystemConfig.FirstOrDefaultAsync(sc => sc.Key == "MaxBlockDocuments");
            if (config == null)
            {
                config = new()
                {
                    Key = "MaxBlockDocuments",
                    Value = "5"
                };
                _context.SystemConfig.Add(config);

               await _context.SaveChangesAsync();
            }
            return ResponseResult.CreateResponse<SystemConfig>(true, message.Success.Get, config);
        }
        //set
        public async Task<Response<SystemConfig>> SetMaxBlockDocuments(SystemConfig systemconfig)
        {

            SystemConfig config = await _context.SystemConfig.FirstOrDefaultAsync(sc => sc.Key == systemconfig.Key);

            if (config != null)
            {
                config.Value = systemconfig.Value;
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, message.Success.Set, config);
            }
            return ResponseResult.CreateResponse<SystemConfig>(false, message.Failure.Set);
        }
        //update
        public async Task<Response<SystemConfig>> UpdateMaxDocumentPerBlock(SystemConfig systemconfig)
        {
            var config = await _context.SystemConfig.FindAsync(systemconfig.Id); //Ef rastrea los cambios de entidades cargadas desde el contexto
            if (config != null)
            {
                config.Value = systemconfig.Value;
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, message.Success.Modify, config);
            }

            return ResponseResult.CreateResponse<SystemConfig>(false, message.Failure.Modify);

        }
    }
}
