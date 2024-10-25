using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Services
{
    public class UserService: IUserService
    {
        private readonly BlockChainContext _context;
        private readonly Message message;

        public UserService(BlockChainContext context, MessageService messages)
        {
            _context = context;
            message = messages.Get_Message();
        }

        //Get
        public async Task<Response<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null){ return ResponseResult.CreateResponse(true, message.Success.Get , user); }
            return ResponseResult.CreateResponse<User>(false, message.NotFound);
        }

        //Set/Post
        public async Task<Response<User>> SetUser(User user)
        {
            _context.Users.Add(user);
            int entriesWriten = await _context.SaveChangesAsync();

            if (entriesWriten > 0) { return ResponseResult.CreateResponse<User>(true, message.Success.Set); }
            return ResponseResult.CreateResponse<User>(false, message.Failure.Set);
        }
            //update
        public async Task<Response<User>> UpdateUser(User user)
        {
        var updatedUser = new User
        {
            Id = user.Id,
            UserN = user.UserN,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            Password = user.Password,
        };
        _context.Entry(updatedUser).State = EntityState.Modified;
        int row_affected = await _context.SaveChangesAsync();

        if (row_affected > 0) { return ResponseResult.CreateResponse<User>(true, message.Success.Modify); }
        return ResponseResult.CreateResponse<User>(false, message.Failure.Modify);
        }

        // delete
        public async Task<Response<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, message.Success.Remove, user);
            }
            return ResponseResult.CreateResponse<User>(false, message.Failure.Remove);

        }

        public async Task<Response<User>> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                return ResponseResult.CreateResponse(true, message.Success.Get, user);
            }
            return ResponseResult.CreateResponse<User>(false, message.NotFound);
        }
    }
}
