using BlockChain_DB;

namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}
