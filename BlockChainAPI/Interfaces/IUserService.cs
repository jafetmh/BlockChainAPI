using BlockChain_DB;
using BlockChain_DB.Response;

namespace BlockChainAPI.Interfaces
{
    public interface IUserService
    {
        public Task<Response<User>> GetUser(int id);
        public Task<Response<User>> SetUser(User user);
        public Task<Response<User>> UpdateUser(User user);
        public Task<Response<User>> DeleteUser(int id);
        public Task<Response<User>> ValidateUser(string email, string password);

        //question for the future
        //public Task<Response<User>> ValidateUser(string user, string password);
    }
}
//Need to add other interfaces for other services (ConfigurationService, MemPoolService)....