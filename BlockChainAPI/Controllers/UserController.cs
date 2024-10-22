using BlockChain_DB;
using BlockChainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _user_service;

        public UserController(UserService user_service)
        {
            _user_service = user_service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id) { 
        
            var response = await _user_service.GetUser(id);
            if(response.Success) { return Ok(response.Data); }
            return BadRequest(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult> SaveUser([FromBody] User user)
        {
            var response = await _user_service.SetUser(user);
            if (response.Success) { return Ok(); }
            return StatusCode(500, response.Message);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
        {
            var response = await _user_service.Update_user(user);
            if (response.Success) { return Ok(); };
            return StatusCode(500, response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var response = await _user_service.Delete_user(id);
            if (response.Success) { return Ok(response.Message); }
            return BadRequest(response.Message);
        }
    }
}
