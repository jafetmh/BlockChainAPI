using BlockChain_DB;

namespace BlockChainAPI.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}
