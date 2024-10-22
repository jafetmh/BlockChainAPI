using BlockChain_DB;
using BlockChainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationService _service;

        public ConfigurationController(ConfigurationService service) {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetMaxNumOfDocuments() {
            var maxNumOfDocument = _service.GetMaxBlockDocuments();
            return Ok(maxNumOfDocument);
        }

        [HttpPost("{value}")]
        public IActionResult SetMaxNumOfDocuments( int value)
        {
            _service.SetMaxBlockDocuments(value);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMaxDocConfig([FromBody] SystemConfig sysConfig) { 
            var result = await _service.Update_MaxDocumentPerBlock(sysConfig);
            if(result.Success) return Ok(result.Message);
            return StatusCode(500, result.Message);
        }
    }
}
