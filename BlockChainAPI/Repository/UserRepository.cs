using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IServices.ICrypto.SHA256;
using BlockChainAPI.Interfaces.IServices.Utilities;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BlockChainAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BlockChainContext _context;
        private readonly ISHA256Hash _hash;
        private readonly Message message;

        public UserRepository(BlockChainContext context, ISHA256Hash hash, IMessageService messages)
        {
            _context = context;
            _hash = hash;
            message = messages.Get_Message();
        }

        //Get
        public async Task<Response<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null) { return ResponseResult.CreateResponse(true, message.Success.Get, user); }
            return ResponseResult.CreateResponse<User>(false, message.NotFound);
        }

        //Set/Post
        public async Task<Response<User>> SetUser(User user)
        {
            byte[] salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            user.Salt = salt;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
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
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, message.Success.Remove, user);
            }
            return ResponseResult.CreateResponse<User>(false, message.Failure.Remove);

        }

        public async Task<Response<UserDTO>> Login(string userName, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserN == userName);

                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        UserDTO userDTO = new()
                        {
                            Id = user.Id,
                            UserN = user.UserN,
                            Name = user.Name,
                            LastName = user.LastName,
                            Email = user.Email,
                        };
                        return ResponseResult.CreateResponse(true, message.Success.Get, userDTO);
                    }
                    else
                    {
                        return ResponseResult.CreateResponse<UserDTO>(false, message.InvalidCredential);
                    }
                }

                return ResponseResult.CreateResponse<UserDTO>(false, message.NotFound);
            }
            catch (Exception ex)
            {
                return ResponseResult.CreateResponse<UserDTO>(false, ex.Message);
            }
        }

    }
}
