using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Common.Account.Dtos;
using Project.Core.Entities.Common.User.Dtos;
using Project.Core.Entities.Helper;
using System.Threading.Tasks;

namespace Project.API.Controllers.Dummy
{
    [Route("[controller]")]
    [ApiController]
    public class LearningController : ControllerBase
    {

        [Authorize,HttpGet("Get", Name = "LearningGetById")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost()]
        public async Task<IActionResult> Post(LoginDto model)
        {
            return BadRequest(new ApiResponse() { Message = "Something Wrong"});
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
