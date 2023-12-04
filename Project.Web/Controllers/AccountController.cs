using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Common.Account.Dtos;
using System.Security.Claims;

namespace Project.Web.Controllers
{
    public class AccountController : BaseController
    {
        public readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model,string returnUrl= "/Home/Index")
        {
            string cookieName = _configuration.GetValue<string>("Constants:CookieName");

            var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, model.Username, ClaimValueTypes.String, cookieName)
                        };

            var userIdentity = new ClaimsIdentity(claims, cookieName);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(cookieName, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddSeconds(18000),
                IsPersistent = false,
                AllowRefresh = true
            });

            return RedirectPermanent(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(_configuration.GetValue<string>("Constants:CookieName"));
            return RedirectToAction("Index", "Home");
        }
    }
}
