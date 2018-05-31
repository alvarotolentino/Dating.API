using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dating.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDataRepository _repository;

        public ValuesController(IDataRepository repository)
        {
            _repository = repository;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var result = _repository.Get();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _repository.RetriveById(id);
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
