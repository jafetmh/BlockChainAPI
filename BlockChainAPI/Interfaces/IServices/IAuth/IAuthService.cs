using BlockChain_DB;
using BlockChain_DB.DTO;

namespace BlockChainAPI.Interfaces.IServices.IAuth
{
    public interface IAuthService
    {
        string GenerateToken(UserDTO user);
    }
}
