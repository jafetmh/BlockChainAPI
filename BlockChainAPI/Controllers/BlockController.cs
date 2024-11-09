using BlockChain_DB;
using BlockChain_DB.DTO;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Interfaces.IDataService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using System.Threading.Tasks;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IBlockService _miningService;
        private readonly IUserRepository _user_service;

        public BlockController(IBlockService miningService, IUserRepository userService)
        {
            _miningService = miningService;
            _user_service = userService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> MineBlock(int userId, [FromBody] List<Document> documents)
        {
            try
            {
                Response<User> result = await _user_service.GetUser(userId);
                if (!result.Success) { return StatusCode(500, result); }
                await _miningService.BuildBlock(result.Data, documents);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest($"Error mining block: {ex.Message}");
            }
        }
    }
}
