using HeidelbergCement.Data.DTO;
using HeidelbergCement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HeidelbergCement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogProxyController : ControllerBase
    {
        readonly IAirTableDataProvider _airTableDataProvider;
        readonly IConfiguration _configuration;

        public LogProxyController(IAirTableDataProvider airTableDataProvider, IConfiguration configuration)
        {
            _airTableDataProvider = airTableDataProvider;
            _configuration = configuration;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Basic")]
        public IActionResult GetRecord()
        {
            var url = GetURL(_configuration);
            if (!string.IsNullOrEmpty(url))
            {
                var records = _airTableDataProvider.GetRecords(url);
                return Ok(records);
            }
            else
            {
                return BadRequest("Url is missing!");
            }
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Basic")]
        public IActionResult PostRecord([FromBody] RequestRecord requestRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (requestRecord?.records == null)
            {
                return BadRequest("The input format is incorrect!");
            }
            var url = GetURL(_configuration);
            if (!string.IsNullOrEmpty(url))
            {
                var records = _airTableDataProvider.PostRecord(url, requestRecord);
                return Ok(records);
            }
            else
            {
                return BadRequest("Url is missing!");
            }
        }
        private string GetURL(IConfiguration configuration)
        {
            return _configuration.GetSection("AirTableUrl").GetSection("url").Value;
        }

    }
}
