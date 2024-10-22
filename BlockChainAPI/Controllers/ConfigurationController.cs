using BlockChain_DB;
using BlockChainAPI.Interfaces;
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

        public ConfigurationController(ConfigurationService service) {
            _configurationService = service;
        }

        [HttpGet]
        public IActionResult GetMaxNumOfDocuments() {
            var maxNumOfDocument = _configurationService.GetMaxBlockDocuments();
            return Ok(maxNumOfDocument);
        }

        [HttpPost("{value}")]
        public IActionResult SetMaxNumOfDocuments( int value)
        {
            _configurationService.SetMaxBlockDocuments(value);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMaxDocConfig([FromBody] SystemConfig sysConfig) { 
            var result = await _configurationService.Update_MaxDocumentPerBlock(sysConfig);
            if(result.Success) return Ok(result.Message);
            return StatusCode(500, result.Message);
        }
    }
}
