using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Comment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            _logger.LogInformation("Logging custom information message");

            return "TestData";
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult Error()
        {
            throw new NotImplementedException("Ooops, error");
        }


        [HttpGet]
        [Route("[action]")]
        public ActionResult Fatal()
        {
           _logger.LogCritical("Fatal error");

           return BadRequest("Fatal error");
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult ErrorWithCatch()
        {
            try
            {
                throw new Exception("Ooops, error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logging custom exception");
                return BadRequest("Ooops, error");
            }
        }
    }
}
