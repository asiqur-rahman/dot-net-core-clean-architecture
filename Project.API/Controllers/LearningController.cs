using Microsoft.AspNetCore.Mvc;

namespace Project.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        [HttpGet("Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}", Name = "HAHA")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("asd")]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
