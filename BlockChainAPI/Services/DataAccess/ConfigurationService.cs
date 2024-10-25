
using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;

namespace BlockChainAPI.Services
{
    public class ConfigurationService: IConfigurationService
    {
        private readonly BlockChainContext _context;
        private readonly Message message;


        public ConfigurationService(BlockChainContext context, MessageService messages)
        {
            _context = context;
            message = messages.Get_Message();
        }

        //get
        public Response<SystemConfig> GetMaxBlockDocuments()
        {
            var config = _context.SystemConfig.FirstOrDefault(x => x.Key == "MaxBlockDocuments");
            if (config != null) { return ResponseResult.CreateResponse(true, message.Success.Get, config); }
            return ResponseResult.CreateResponse<SystemConfig>(false, message.Failure.Get);
        }
        //set
        public Response<SystemConfig> SetMaxBlockDocuments(int value) {

            var config = _context.SystemConfig.FirstOrDefault(x => x.Key == "MaxBlockDocuments");

            if (config != null)
            {
                config.Value = value.ToString();
                _context.SaveChanges();
            }
            else
            {
                _context.SystemConfig.Add(new SystemConfig
                {
                    Key = "MaxBlockDocuments",
                    Value = value.ToString()
                });

                _context.SaveChanges();
            }
            return ResponseResult.CreateResponse<SystemConfig>(true, message.Success.Set);
        }
        //update
        public async Task<Response<SystemConfig>> UpdateMaxDocumentPerBlock(SystemConfig sysconfig)
        {
            var config = await _context.SystemConfig.FindAsync(sysconfig.Key); //Ef rastrea los cambios de entidades cargadas desde el contexto
            if (config != null) {
               config.Value = sysconfig.Value;
               await _context.SaveChangesAsync();
               return ResponseResult.CreateResponse(true, message.Success.Modify, config);
            }

            return ResponseResult.CreateResponse<SystemConfig>(false, message.Failure.Modify);
            
        }
    }
}
