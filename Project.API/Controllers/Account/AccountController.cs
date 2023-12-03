using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Common.Account.Dtos;

namespace Project.API.Controllers.Account
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("Login", Name = "UserLogin")]
        public string Login(LoginDto model)
        {
            return "value";
        }
    }
}
