using BlockChain_DB;
using BlockChain_DB.Response;


namespace BlockChainAPI.Interfaces
{
    public interface IConfigurationService

    {
        public int GetMaxBlockDocuments();
        public void SetMaxBlockDocuments(int value);
        public Task<Response<SystemConfig>> UpdateMaxDocumentPerBlock(SystemConfig sysconfig);



    }
}