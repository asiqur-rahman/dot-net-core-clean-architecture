using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.Core.Config;
using Project.Core.Entities.Common.Account.Dtos;
using Project.Core.Entities.Common.Security;
using System.Security.Claims;
using System.Text.Json;

namespace Project.Web.Controllers
{
    public class AccountController : BaseController
    {
        public readonly IConfiguration _configuration;
        public readonly AppSettings _appSettings;
        public AccountController(IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model,string returnUrl= "/Home/Index")
        {
            string cookieName = _appSettings.Cookie.Name;

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

            UserPrincipal cookiesData = new UserPrincipal();
            cookiesData.Username = model.Username;
            HttpContext.Session.SetString("UserSessionData", JsonSerializer.Serialize(cookiesData));

            return RedirectPermanent(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(_appSettings.Cookie.Name);

            return RedirectToAction("Login");
        }
    }
}
