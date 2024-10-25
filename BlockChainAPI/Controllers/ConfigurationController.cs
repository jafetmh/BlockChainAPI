using BlockChain_DB;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configService) {
            _configurationService = configService;
        }

        [HttpGet]
        public IActionResult GetMaxNumOfDocuments() {
            var response = _configurationService.GetMaxBlockDocuments();
            if (response.Success) { return Ok(response); }
            return BadRequest(response);
        }

        [HttpPost("{value}")]
        public IActionResult SetMaxNumOfDocuments( int value)
        {
            var response =_configurationService.SetMaxBlockDocuments(value);
            if(response.Success) { return Ok( response); }
            return StatusCode(500, response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMaxDocConfig([FromBody] SystemConfig sysConfig) { 
            var result = await _configurationService.UpdateMaxDocumentPerBlock(sysConfig);
            if(result.Success) return Ok(result.Message);
            return StatusCode(500, result.Message);
        }
    }
}
