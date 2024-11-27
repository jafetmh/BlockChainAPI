using BlockChain_DB;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemLogController : ControllerBase
    {
        private readonly BlockChainContext _context;

        public SystemLogController(BlockChainContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            List<SystemLog> logs = await _context.SystemLog
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
            return Ok(logs);
        }


    }
}
