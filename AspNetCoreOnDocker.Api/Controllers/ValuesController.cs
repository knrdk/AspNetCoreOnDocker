using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreOnDocker.MongodDb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreOnDocker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ILogger<ValuesController> _logger;
        private ValuesDal valuesDal;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
            valuesDal = new ValuesDal();
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return valuesDal.GetAllValues();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogDebug("Getting value. Id: {id}", id);
            if(id>2){
                _logger.LogWarning("An error occured while getting value. Id: {id}", id);
            }
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            valuesDal.AddValue(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
