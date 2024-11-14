using BlockChain_DB;
using BlockChain_DB.Response;


namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IConfigurationRepository

    {
        public Task<Response<SystemConfig>> GetMaxBlockDocuments();
        public Task<Response<SystemConfig>> SetMaxBlockDocuments(SystemConfig systemconfig);
        public Task<Response<SystemConfig>> UpdateMaxDocumentPerBlock(SystemConfig systemconfig);

    }
}