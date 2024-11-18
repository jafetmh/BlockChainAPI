using BlockChain_DB;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationRepository _configurationService;

        public ConfigurationController(IConfigurationRepository configService) {
            _configurationService = configService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxNumOfDocuments() {
            Response<SystemConfig> response = await _configurationService.GetMaxBlockDocuments();
            if (response.Success) { return Ok(response); }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> SetMaxNumOfDocuments([FromBody] SystemConfig config)
        {
            Response<SystemConfig> response = await _configurationService.SetMaxBlockDocuments(config);
            if(response.Success) { return Ok( response); }
            return StatusCode(500, response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMaxDocConfig([FromBody] SystemConfig config) {
            Response<SystemConfig> result = await _configurationService.UpdateMaxDocumentPerBlock(config);
            if(result.Success) return Ok(result);
            return StatusCode(500, result);
        }
    }
}
