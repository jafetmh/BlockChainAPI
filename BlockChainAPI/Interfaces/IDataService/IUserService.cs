using BlockChain_DB;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces.IDataService
{
    public interface IUserService
    {
        public Task<Response<User>> GetUser(int id);
        public Task<Response<User>> SetUser(User user);
        public Task<Response<User>> UpdateUser(User user);
        public Task<Response<User>> DeleteUser(int id);
        public Task<Response<User>> Login(string email, string password);
    }
}