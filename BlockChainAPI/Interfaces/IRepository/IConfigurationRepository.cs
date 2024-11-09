using BlockChain_DB;
using BlockChain_DB.Response;


namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IConfigurationRepository

    {
        public Response<SystemConfig> GetMaxBlockDocuments();
        public Response<SystemConfig> SetMaxBlockDocuments(int value);
        public Task<Response<SystemConfig>> UpdateMaxDocumentPerBlock(SystemConfig sysconfig);

    }
}