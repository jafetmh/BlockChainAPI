using BlockChain_DB;
using BlockChain_DB.Response;


namespace BlockChainAPI.Interfaces
{
    public interface IConfigurationService
    {
        public Task<Response<Configuration>> GetConfiguration(int id);
        public Task<Response<Configuration>> SetConfiguration(Configuration configuration);
        public Task<Response<Configuration>> UpdateConfiguration(Configuration configuration);

        //public Task<Response<Configuration>> DeleteConfiguration(int id);



    }
}