using BlockChain_DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private BlockChainContext _context;

        public UserController(BlockChainContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> Get() => _context.Users.ToList();
    }
}
