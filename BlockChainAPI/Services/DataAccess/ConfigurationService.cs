
using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Utilities;

namespace BlockChainAPI.Services
{
    public class ConfigurationService: IConfigurationService
    {
        private readonly BlockChainContext _context;


        public ConfigurationService(BlockChainContext context)
        {
            _context = context;
        }

        //get
        public int GetMaxBlockDocuments()
        {
            var config = _context.SystemConfig.FirstOrDefault(x => x.Key == "MaxBlockDocuments");
            return config != null ? int.Parse(config.Value) : 0;
        }
        //set
        public void SetMaxBlockDocuments(int value) {
            var config = _context.SystemConfig.FirstOrDefault(x => x.Key == "MaxBlockDocuments");

            if (config != null)
            {
                config.Value = value.ToString();
                _context.SaveChanges();
            }
            else
            {
                //if it doesn't exist, create it
                _context.SystemConfig.Add(new SystemConfig
                {
                    Key = "MaxBlockDocuments",
                    Value = value.ToString()
                });

                _context.SaveChanges();
            }
        }
        //update
        public async Task<Response<SystemConfig>> UpdateMaxDocumentPerBlock(SystemConfig sysconfig)
        {
            var config = await _context.SystemConfig.FindAsync(sysconfig.Key); //Ef rastrea los cambios de entidades cargadas desde el contexto
            if (config != null) {
               config.Value = sysconfig.Value;
               await _context.SaveChangesAsync();
               return ResponseResult.CreateResponse(true, "Actualizado con exito", config);
            }

            return ResponseResult.CreateResponse<SystemConfig>(false,"Error al actualizar");
            
        }
    }
}
