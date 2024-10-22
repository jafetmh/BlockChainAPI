using BlockChain_DB;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces
{
    public interface IUserService
    {
        public Task<Response<User>> GetUser(int id);
        public Task<Response<User>> SetUser(User user);
        public Task<Response<User>> Update_user(User user);
        public Task<Response<User>> Delete_user(int id);
    }
}
