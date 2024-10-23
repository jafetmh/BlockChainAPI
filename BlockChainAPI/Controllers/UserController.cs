using BlockChain_DB;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]//name controller minuscula/id
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;//change UserService to IUserService and the other controllers

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id) { 
        
            var response = await _userService.GetUser(id);
            if(response.Success) { return Ok(response.Data); }
            return BadRequest(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult> SaveUser([FromBody] User user)
        {
            var response = await _userService.SetUser(user);
            if (response.Success) { return Ok(); }
            return StatusCode(500, response.Message);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
        {
            var response = await _userService.UpdateUser(user);
            if (response.Success) { return Ok(); };
            return StatusCode(500, response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.Success) { return Ok(response.Message); }
            return BadRequest(response.Message);
        }


        //call method validate user
        [HttpPost("validate")]
        public async Task<ActionResult> ValidateUser([FromBody] User user)
        {
            var response = await _userService.ValidateUser(user.Email, user.Password);
            if (response.Success) {
                //token
                return Ok(response.Data);

            }
                return BadRequest(response.Message);
        }
        //add method post recibe parameter user and password and compare with database
    }
}
