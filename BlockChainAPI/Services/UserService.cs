﻿using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Services
{
    public class UserService: IUserService
        //make a equals methods for others services
    {
        private readonly BlockChainContext _context;//context database (models and tables)

        public UserService(BlockChainContext context)
        {
            _context = context;
        }

        //Get
        public async Task<Response<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null){ return ResponseResult.CreateResponse(true, "Se ha obtenido con exito", user); }
            return ResponseResult.CreateResponse<User>(false, "Error al obtener");
        }

        //Set/Post
        public async Task<Response<User>> SetUser(User user)
        {  
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return ResponseResult.CreateResponse<User>(true, "Se creo con exito");           
        }

        //update
        public async Task<Response<User>> UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            int row_affected = await _context.SaveChangesAsync();
            if (row_affected > 0) { return ResponseResult.CreateResponse<User>(true, "Se actualizo correctamente"); }
            return ResponseResult.CreateResponse<User>(false, "No se realizaron cambios");
        }

        // delete
        public async Task<Response<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return ResponseResult.CreateResponse(true, "Eliminado con exito", user);
            }
            return ResponseResult.CreateResponse<User>(false, "Usuario no valido");

        }

        //validate user

        public async Task<Response<User>> ValidateUser(string user, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User == user && u.Password == password);
            if (user != null) {
                return ResponseResult.CreateResponse(true, "Usuario valido", user); 
            }
            return ResponseResult.CreateResponse<User>(false, "Usuario no valido");

        }

        //they are all consults to database 


    }
}
