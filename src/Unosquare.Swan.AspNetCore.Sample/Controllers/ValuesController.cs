﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Swan.AspNetCore.Sample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get() => new[] { "value1", "value2" };

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id) => "value";

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
