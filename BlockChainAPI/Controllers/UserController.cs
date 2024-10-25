using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChainAPI.Interfaces.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
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
            if (response.Success) { return Ok(response); }
            return StatusCode(500, response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
        {
            var response = await _userService.UpdateUser(user);
            if (response.Success) { return Ok(response); };
            return StatusCode(500, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.Success) { return Ok(response); }
            return BadRequest(response);
        }


        //login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDTO user)
        {
            var response = await _userService.Login(user.Email, user.Password);
            if (response.Success) {
                //Get Token
                var token = _authService.GenerateToken(response.Data);

                return Ok(new { User = response.Data, Token = token});

            }
            return NotFound(response.Message);
        }
    }
}
