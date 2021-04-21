using Microsoft.AspNetCore.Mvc;
using log4net;
using System.Linq;
using FundaAssignment.Core.Interfaces;
using FundaAssignment.Core.Models;

namespace FundaAssignment.WebApp.Controllers
{
    [ApiController]
    [Route("funda")]
    public class FundaController : ControllerBase
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(FundaController));
        private readonly IService _fundaService;

        public FundaController(IService fundaService)
        {
            _fundaService = fundaService;
        }

        [HttpGet, Route("{type}/{stad}/{buitenruimte?}")]
        public IActionResult Get(string type, string stad, string buitenruimte)
        {
            var request = new RequestModel()
            {
                Type = type,
                Stad = stad,
                Buitenruimte = buitenruimte
            };

            _log.Info($"Requesting Funda with next parameters: Type={request.Type}, Stad={request.Stad}, Buitenruimte={request.Buitenruimte}");
            var result = _fundaService.GetTop10Agencies(request);
            
            if (!result.Any())
                return NotFound();
            
            return Ok(result);
        }
    }
}
